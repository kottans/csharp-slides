- title : Multithreading
- description : Log4Net framework
- author : Valentine Radchuk
- theme : league
- transition : default

***

## C# course
#### Lecture 15
# Loggin' with log4net

***

## Agenda
- Overview. Why we need logging?
- Logging - native VS tools
- Logging frameworks
- Log4net overiew

***
###Logging. Overview

**Logging** - the process of recording application actions and state to a secondary interface.

Second interface could be:

- file
- web service
- external log collector
- database
- e-mail 
- etc ...

---
###Logging intension

- track application state
- inform what system doing right now
- store history of internal appliaction actions
- help to detect problems with an application without debugger
- help analyze system health and botlenecks

***
###A very simple logger

<div>

```cs
Console.WriteLine("This is an log message");
```
</div>

---
###Advanced logger

<div>

```cs
var logMessage = "This is an log message";
var logFile = "logfile.log";

using (FileStream fs = new FileStream(logFile,FileMode.Append, FileAccess.Write))
using (StreamWriter sw = new StreamWriter(fs))
	{
		sw.WriteLine(logMessage);
	}
```
</div>

---
###More advanced technics

####Using class <a href="https://msdn.microsoft.com/en-us/library/system.diagnostics.trace(v=vs.110).aspx">Trace</a> for tracing application state

<div>

```cs
using System;
using System.Diagnostics;

class Test
{
    static void Main()
    {
       Trace.Listeners.Add(new TextWriterTraceListener("yourlog.log"));
       Trace.AutoFlush = true;
       Trace.WriteLine("Entering Main");
       Console.WriteLine("Hello World.");
       Trace.WriteLine("Exiting Main"); 
       Trace.Flush();
    }
}
```
</div>

***
###Structured approach to logging

- introduction of logging level
- change logging interface or support few ones at the time
- keep important information with each log message
- filter log messages depending on environment
- support multiple loggers in an application
- use logging frameworks

***
###Logging levels
What information might be interesting for those who will read log?

<section data-markdown>
    <script type="text/template">
	- DEBUG - any debug information <!-- .element: class="fragment" data-fragment-index="1" -->  
	- INFO - general comments or intermediate calculations <!-- .element: class="fragment" data-fragment-index="2" -->  
	- WARN - warning, something potentially goes wrong <!-- .element: class="fragment" data-fragment-index="3" -->	  
	- ERROR - undesired behaviour/incorrect answer from service, any problem that won't crash application <!-- .element: class="fragment" data-fragment-index="4" -->  
	- FATAL - critical problem. Application cannot continue running <!-- .element: class="fragment" data-fragment-index="5" -->	
    </script>
</section>

---
###Change logging interface
Depending on application or environment we might be interesting to find logs in different places:   

- Console - for quick and easily access for log messages 
- File - for store big and huge logs 
- Database - to get structured logs 
- Email - to get notification if wmth goes wrong 
- Web Service - pass log data to a service for further transformation or analizing 

---
###Keep important information

What might be important for those who read logs?

- date and time in different formats
- class/method name which produced a log message
- log level
- thread id (for multithreaded applications)
- user/machine name

---
###Filter log messages
What if only subset of generated logs required for now?
In this case filters might be applied

- filter by level
- filter by substring in message

---
###Log to multiple interfaces simultaneously

Applying filters and log levels log messages might be sent to different interfaces

####Possble scenarios:
[INFO] -> Datadase   
[DEBUG] -> File   
[ERROR, FATAL] -> EMail   

***
###2 log || ! 2 log
Logs are useful but they might be excessive:  

- log instructions take CPU time and extra logging will slow down performance
- long and detailed log is hard for analysis
- log only relative information
- log your environment
- carefully choose log level

***
###Logging frameworks 
Threre are numerous of logging frameworks:

- <a href="https://logging.apache.org/">log4net</a> - powerful, most famous, ported from log4j
- <a href="http://nlog-project.org/">NLog</a> - flexible adn powerful framework, actively developed 
- <a href="https://msdn.microsoft.com/en-us/library/ff647183.aspx">Enterprise Log</a> - dunno for whom this logger :)
- dozens of other frameworks

***
###log4net overview

Core features:

- Support for multiple frameworks
- Output to multiple logging targets (20 standard appenders)
- Hierarchical logging architecture (each component has own logger)
- XML Configuration (can be configured either via app.config or external xml file)
- Dynamic Configuration (updated configuration loaded on the fly)
- Logging Context (additional contextual data for each log message) 
- Modular and extensible design
- High performance with flexibility

---
###Core concepts
Log4net has 3 main components:

- loggers - named entities which are used to write log messages
- appenders - endpoint interfaces to write log to particular destination
- layouts - sets shape of each message

***
###Loggers
