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

<a href="https://logging.apache.org/">Log4net</a> is an <a href="https://github.com/apache/log4net">open source </a> logging framework

Core features:

- Support for multiple frameworks
- Output to multiple logging targets (20 standard appenders)
- Hierarchical logging architecture (component has own logger)
- XML Configuration (app.config or external xml file)
- Dynamic Configuration (configuration loaded on the fly)
- Logging Context (contextual data for each log message) 
- Modular and extensible design
- High performance with flexibility

---
###Core concepts
Log4net has 4 main components:

- loggers - named entities which are used to write log messages
- appenders - endpoint interfaces to write log to particular destination
- filters - to filter out messages by a condition
- layouts - sets shape of each message

***
###Loggers

- loggers - named entities encapsulate log target
- loggers support hierarchy
- you can create as many loggers as you need
- log level can be configured per logger
- loggers implement interface ILog with following members:

```cs                                           
public interface ILog
{
  void Debug(object message);
  void Info(object message);
  void Warn(object message);
  void Error(object message);
  void Fatal(object message);
//...
  void Debug(object message, Exception ex);
//...
  bool isDebugEnabled;
}
```
---
###Root logger

- default parent logger
- cannot be accessed directly
- all setting enabled for root logger are accessible for all ancestors

####Configuration sample

```xml                                           
<root>
  <level value="INFO"/>
  <appender-ref ref="FileAppender"/>
  <appender-ref ref="ConsoleAppender" />
</root>
```
---
###Logger hierarchy

Log4net supports hierarchy of loggers   
Hierarchy concepts uses C# namespaces 

- loggers use dot-notation e.g. Namespace.Class
- attributes applied for parent logger applies for child
- attributes applied to child logger overrides parent ones

---
###Recomendation for logger usage

- configure loggers in app.config or external config file
- create loger per class to leverage hierarchy configuration of log4net
- do not pass logger as a parameter to constructor excepting cases when you really need this

---
###Example of usage logger

####Instantiate logger
```cs
using log4net;
namespace Com.Foo {
    public class Bar {
        private static readonly ILog log = LogManager.GetLogger(typeof(Bar));        
        public void DoIt() {
            log.Debug("Oops, I did it again!");
        }
    }
}
```
####Enable log4net in Assembly.cs
```cs
// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch=true)]
// Configure log4net using the .log4net file
[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension="log4net",Watch=true)]
```
---
###Configuration syntax in app.config
```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <section name="log4net" 
		type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
    </configSections>
    <log4net>
        <appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
            <layout type="log4net.Layout.PatternLayout">
                <conversionPattern 
		value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
            </layout>
        </appender>
        <root>
            <level value="INFO" />
            <appender-ref ref="ConsoleAppender" />
        </root>
    </log4net>
</configuration>
```
<a href="http://logging.apache.org/log4net/release/manual/configuration.html">More configuraion samples</a>

***
###Appenders

Appenders specify where and how the information will be logged

- appenders is a named entity with a type
- appenders might be attached to loggers by many-to-many relationship

```xml
<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender" >
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern 
	value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
    </layout>
</appender>
```

---
###Frequesntly used appenders
- ConsoleAppender - writes to console
- ColoredConsoleAppender - colored messages to console
- RollingFileAppender - messages to file(s) with date/filesize constraints
- SmtpAppender - sends log messages to email

<a href="https://logging.apache.org/log4net/release/manual/introduction.html#appenders">More standard appenders</a> and <a href="http://logging.apache.org/log4net/release/config-examples.html">config samples</a>

***
###Filters

- filters are specified for appenders
- few filters can be applied to a appender. Order matters
- filter by level, level range, keyword or deny all filters
- filters make chaining. Once filter match record, record will be printed, otherwise passed to next filter. 
- by default all filters pass record. To stop message propagation use DenyAllFilters

```xml
<appender name="ConsoleAppender" type="log4net.Appender.ConsoleAppender">
  <filter type="log4net.Filter.StringMatchFilter">
      <stringToMatch value="database" />
  </filter>  
  <filter type="log4net.Filter.DenyAllFilter" />
</appender>
```

More about <a href="http://logging.apache.org/log4net/release/manual/configuration.html#filters">configuration</a> and <a href="https://logging.apache.org/log4net/release/manual/introduction.html#filters">filters</a>

***
###Layouts

- shoule be defined for each appender
- specifies how log data should be written

```xml
<layout type="log4net.Layout.PatternLayout">
  <conversionPattern 
   value="%date [%thread] %-5level %logger [%ndc] - %message%newline"/>
</layout>
```

More info about <a href="https://logging.apache.org/log4net/release/manual/introduction.html#layouts">predefined layouts</a> and <a href="http://logging.apache.org/log4net/release/manual/configuration.html#layouts">configuration</a>

---
###Conversion patterns of pattern layout

- **%date** - outputs local time
- **%level** - log level
- **%message** - outputs log message
- **%exception** - if exception passed, will be formated from new line
- **%newline** - outputs new line 
- **%identity** - outputs current user identity
- **%location** - outputs location of log invocation (useful in Debug mode)

<a href="https://logging.apache.org/log4net/release/sdk/log4net.Layout.PatternLayout.html">More patterns</a>