# Diagnostics.NET

Classes for debug and diagnostic operations written in C#.

## Branches

| Name  | Targets on  |
| ----- | ----------- |
| [master](https://github.com/mkloubert/Diagnostics.NET)  | C# 4.0  |
| [CSharp5](https://github.com/mkloubert/Diagnostics.NET/tree/CSharp5)  | C# 5.0, .NET 4.5  |
| [NetCore5](https://github.com/mkloubert/Diagnostics.NET/tree/NetCore5)  | C# 6.0, .NET Core 5  |
| Portable8 (current)  | C# 4.0, .NET 4.5, Silverlight 5, Windows 8, Windows Phone 8.1 + 8 (Silverlight)  |

## Loggers

### AggregateLogger

Executes a list of loggers.

```csharp
var subLogger1 = new NullLogger();

var subLogger2 = new EventLogger();
subLogger2.MessageReceived += (sender, e) =>
    {
        //TODO
    };


var logger = new AggregateLogger();
logger.AddLogger(subLogger1);
logger.AddLogger(subLogger2);
```

### AsyncLogger

Wraps a logger and runs each log operation of it in background.

### DelegateLogger

Uses a delegate to handle a log operation.

```csharp
static class LogHandler {
    public static void MyLogAction(DelegateLogger logger, ILogMessage msg, ref bool success) {
        //TODO
    }
}

var logger = new DelegateLogger(LogHandler.MyLogAction);
```

### EventLogger

Fires an event when a log operation is made.

```csharp
var logger = new EventLogger();
logger.MessageReceived += (sender, e) =>
    {
        // e.Message contains the log message
    };
```

### FallbackLogger

Wraps a main logger and uses an alternate logger if that main logger fails.

```csharp
// the main logger
var mainLogger = new EventLogger();
mainLogger.MessageReceived += (sender, e) =>
    {
        // lets make this logger fail
        throw new NotImplementedException();
    };

// the fallback logger
var alternateLogger = new TextLogger(Console.Out);

var logger = new FallbackLogger(mainLogger,
                                alternateLogger);
```

### NullLogger

A logger that does nothing.

### SynchronizedLogger

A thread safe logger.

### TextLogger

Uses a [TextWriter](https://msdn.microsoft.com/en-us/library/system.io.textwriter(v=vs.110).aspx) or a [Stream](https://msdn.microsoft.com/en-us/library/system.io.stream%28v=vs.110%29.aspx) instance to write a log message.

```csharp
// write to console
var consoleLogger = new TextLogger(Console.Out);

// write to file
var fileLogger = new TextLogger(new FileStream("./logfile.txt"));
```
