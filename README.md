# Visual Stack Trace Explorer

Visual Stack Trace Explorer is a developer-focused tool that parses raw **.NET stack traces** and turns them into a **clear, structured, and readable visual call flow**.

Instead of scrolling through walls of text, you immediately see:
- What went wrong (root exception)
- Where it happened (your code)
- How execution reached that point

Built with **C# and Blazor**, this project focuses on real-world stack traces from ASP.NET, Blazor Server, async code, and Entity Framework.

---

## Features

- **Root cause detection**  
  Extracts the actual exception (e.g. `NullReferenceException`, `ArgumentNullException`) as a clear top-level cause.

- **Stack frame parsing**  
  Parses method name, file path, and line number from standard .NET stack traces.

- **User code vs framework code**  
  Automatically highlights your application code and separates framework noise.
  
---

## How it works

1. Paste a raw .NET stack trace into the input
2. The parser:
   - Extracts the **main exception**
   - Identifies valid stack frames (`at ...`)
   - Filters and classifies user code
3. The UI renders the result as a structured, readable call stack
