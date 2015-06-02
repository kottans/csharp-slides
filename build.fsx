﻿#I @"packages/FsReveal/fsreveal/"
#I @"packages/FAKE/tools/"
#I @"packages/Suave/lib/net40"

#r "FakeLib.dll"
#r "suave.dll"

#load "fsreveal.fsx"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "Kottans"
let gitHome = "https://github.com/" + gitOwner
// The name of the project on GitHub
let gitProjectName = "csharp-slides"

open FsReveal
open Fake
open Fake.Git
open Fake.XMLHelper
open System.IO
open System.Diagnostics
open Suave
open Suave.Web
open Suave.Http
open Suave.Http.Files

let toWebCompliantName (original:string) = 
    original.Replace(" ", "-").ToLower()

let topic = (getBuildParam "slides").Trim '"'
let topicWebCompliant = topic |> toWebCompliantName
let outDir = __SOURCE_DIRECTORY__ @@ "output" @@ topicWebCompliant
let slidesDir = __SOURCE_DIRECTORY__ @@ "slides" @@ topic

let ghPagesDir = __SOURCE_DIRECTORY__ @@ "gh-pages"
let publishDir = ghPagesDir @@ "presentations" @@ topicWebCompliant

Target "Clean" (fun _ ->
    CleanDirs [outDir]
)

let fsiEvaluator = 
    let evaluator = FSharp.Literate.FsiEvaluator()
    evaluator.EvaluationFailed.Add(fun err -> 
        traceImportant <| sprintf "Evaluating F# snippet failed:\n%s\nThe snippet evaluated:\n%s" err.StdErr err.Text )
    evaluator 

let copyStylesheet() =
    try
        CopyFile (outDir @@ "css" @@ "custom.css") (slidesDir @@ "custom.css")
    with
    | exn -> traceImportant <| sprintf "Could not copy stylesheet: %s" exn.Message

let copyPics() =
    try
      CopyDir (outDir @@ "images") (slidesDir @@ "images") (fun f -> true)
    with
    | exn -> traceImportant <| sprintf "Could not copy picture: %s" exn.Message    

let generateFor (file:FileInfo) = 
    try
        copyPics()
        let rec tryGenerate trials =
            try
                FsReveal.GenerateFromFile(file.FullName, outDir, fsiEvaluator = fsiEvaluator)
            with 
            | exn when trials > 0 -> tryGenerate (trials - 1)
            | exn -> 
                traceImportant <| sprintf "Could not generate slides for: %s" file.FullName
                traceImportant exn.Message

        tryGenerate 3

        copyStylesheet()
    with
    | :? FileNotFoundException as exn ->
        traceImportant <| sprintf "Could not copy file: %s" exn.FileName

let handleWatcherEvents (events:FileChange seq) =
    for e in events do
        let fi = fileInfo e.FullPath
        traceImportant <| sprintf "%s was changed." fi.Name
        match fi.Attributes.HasFlag FileAttributes.Hidden || fi.Attributes.HasFlag FileAttributes.Directory with
        | true -> ()
        | _ -> generateFor fi

let startWebServer () =
    let serverConfig = 
        { defaultConfig with
           homeFolder = Some (FullName outDir)
        }
    let app =
        Writers.setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
        >>= Writers.setHeader "Pragma" "no-cache"
        >>= Writers.setHeader "Expires" "0"
        >>= browseHome
    startWebServerAsync serverConfig app |> snd |> Async.Start
    Process.Start "http://localhost:8083/index.html" |> ignore

Target "GenerateSlides" (fun _ ->
    !! (slidesDir @@ "*.md")
      ++ (slidesDir @@ "*.fsx")
    |> Seq.map fileInfo
    |> Seq.iter generateFor
)

Target "KeepRunning" (fun _ ->    
    use watcher = !! (slidesDir + "/**/*.*") |> WatchChanges (fun changes ->
         handleWatcherEvents changes
    )
    
    startWebServer ()

    traceImportant "Waiting for slide edits. Press any key to stop."

    System.Console.ReadKey() |> ignore

    watcher.Dispose()
)

Target "SetReleaseTemplate" (fun _ ->
    let templateFile = __SOURCE_DIRECTORY__ @@ "shared" @@ "template.html"
    FsRevealHelper.TemplateFile <- templateFile
)

Target "ReleaseSlides" (fun _ ->
    if gitOwner = "myGitUser" || gitProjectName = "MyProject" then
        failwith "You need to specify the gitOwner and gitProjectName in build.fsx"
    
    DeleteDir ghPagesDir

    Repository.cloneSingleBranch "" (gitHome + "/" + gitProjectName + ".git") "gh-pages" ghPagesDir

    let outputImagesDir = outDir @@ "images"
    let publishImagesDir = publishDir @@ "images"
    if TestDir outputImagesDir then
        CreateDir publishImagesDir
        CleanDir publishImagesDir
        CopyRecursive outputImagesDir publishImagesDir true |> tracefn "%A"

    CopyFile publishDir (outDir @@ "index.html") |> tracefn "%A"
        
    StageAll ghPagesDir
    Git.Commit.Commit ghPagesDir (sprintf "Update generated slides for %s" topic)
    Branches.push ghPagesDir
)



"Clean"
  ==> "GenerateSlides"
  ==> "KeepRunning"

"SetReleaseTemplate"
  <=> "GenerateSlides"
  ==> "ReleaseSlides"
  
RunTargetOrDefault "KeepRunning"