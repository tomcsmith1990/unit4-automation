function Restore {
    nuget.exe restore .
}

function Build {
    Restore
    msbuild.exe .\Unit4.sln

    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4\bin\Debug\unit4-automation.exe --where="cat == RequiresExcelInstall"
}

function Test {
    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4\bin\Debug\unit4-automation.exe
}

function Run {
    & ".\Unit4\bin\Debug\unit4-automation.exe"
}

export-modulemember -function Restore
export-modulemember -function Build
export-modulemember -function Test
export-modulemember -function Run