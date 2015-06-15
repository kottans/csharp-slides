csharp-slides
=============
## Currently CSharp slides are in development and accesible via links:

[Public slides](http://kottans.org/csharp-slides/)   
[Wiki](https://github.com/Kottans/csharp-slides/wiki)    
[Course program](https://docs.google.com/document/d/19Gfj71qbpKlHXIiye7m_PZDk4jA7M1G21bxRzauVP8w/edit#)   

- [x] [Lecture 1 - .NET, infra, tools, basics](https://drive.google.com/open?id=1HMd2B0FErbFMT2v7Z4SDXhUHX_x6fkXMf2QS9yV4X3A&authuser=0)
- [x] [Lecture 2 - C# language essentials](https://drive.google.com/open?id=1-0KDxmndJU2j483mBdqipdx1b4KdZvgncVraBUvqcu0&authuser=0)
- [x] [Lecture 3 - Custom types](http://kottans.org/csharp-slides/presentations/3-custom-types/#/)
- [x] [Lecture 4 - Flow control, Exceptions](https://drive.google.com/open?id=113dEHoylhsbnGlqcXI14O2B6EnFNCWV1UKFBfLilmCo&authuser=0)
- [x] [Lecture 5 - OOP](https://drive.google.com/open?id=17vL1XaJjSr77T18SdaAXP1mquSqVtaI8EHrKwagwuBU&authuser=0)
- [x] [Lecture 5.1 - OOP. Continued](https://drive.google.com/open?id=17ax8dZVzRrsEh53SlzBm2RsalZFvGpRrRJ_YI-QU3EI&authuser=0)
- [ ] Lecture 6 - Working with strings and text
- [x] [Lecture 7 - Delegates, Events and Lambdas](https://drive.google.com/open?id=18aWj-E13-jcpo7Za-RlsZhyRGr2sPPNm9MGKhWinS20&authuser=0)
- [x] [Lecture 8 - Collections and generics](https://drive.google.com/open?id=1x_GryQXEDeWE6vE6X7St8epIFr7cnfgl3eofp9ITqhw&authuser=0)
- [x] [Lecture 9 - LINQ Pt.1] (http://kottans.org/csharp-slides/presentations/10-linq-pt1/#/)
- [x] [Lecture 10 - LINQ Pt.2] (http://kottans.org/csharp-slides/presentations/11-linq-pt2/#/)
- [x] [Lecture 11 - Garbage collection](https://docs.google.com/presentation/d/1n_K-LopzpyI5hMPMftK5WQdULmTUsZxBQ5TtER118FM/edit?usp=sharing)
- [x] Lecture 12 - Reflection
- [ ] Lecture 13 - Multithreading Pt.1
- [ ] Lecture 14 - Multithreading Pt.2   
- [ ] Lecture 15 - Dynamic programming   

----
##How to prepare FsReveal presentation
1. Choose folder for according presentation or create new one
2. Edit content of file `<presentation_folder>\index.md` and add used images to folder `<presentation_folder>\images`
3. Run `build "<presentation_folder>"` in `cmd` to build presentation and raise up local http server and continuous build of markup file to html presentation
4. In order to publish - run `build ReleaseSlides "<presentation_folder>"`
That's it
