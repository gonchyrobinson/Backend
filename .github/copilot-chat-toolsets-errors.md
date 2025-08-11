# Copilot Chat Toolset: Solving Console Errors

Use these Copilot Chat prompts to efficiently diagnose and resolve console errors in this .NET 8, EF Core, MySQL project. These are tailored to your architecture and workflows.

---

## 1. Analyze and Fix Build/Runtime Errors
**Prompt:**
> I received this console error when running/building the project: [paste error message].
> 1. Explain the root cause in the context of this codebase.
> 2. Suggest the minimal code change to fix it, referencing the correct file and line if possible.
> 3. If the error is related to dependency injection, EF Core, or configuration, check `Program.cs`, `appsettings.json`, and relevant service/repository registrations.

## 2. Database Connection/EF Core Errors
**Prompt:**
> I got this EF Core or database error: [paste error message].
> 1. Diagnose the likely cause based on `ApplicationDbContext`, connection strings, and migrations.
> 2. Suggest a fix, referencing the correct config or code file.

## 3. Unhandled Exception or Stack Trace
**Prompt:**
> The console shows this unhandled exception or stack trace: [paste stack trace].
> 1. Trace the error to the relevant service, repository, or controller.
> 2. Suggest a fix, following the project's exception handling and logging patterns.

## 4. Test Failures
**Prompt:**
> A test failed with this output: [paste test failure].
> 1. Explain the failure in the context of the codebase.
> 2. Suggest a targeted code or test fix, referencing the correct file.

---

**How to use:**
- Copy the relevant prompt into Copilot Chat and paste your error message or stack trace.
- The agent will analyze the error using project conventions and suggest actionable fixes.
- For persistent or unclear errors, ask for step-by-step debugging guidance.

For more on project structure and conventions, see `.github/copilot-instructions.md`.
