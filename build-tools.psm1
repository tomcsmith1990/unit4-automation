function Restore {
    nuget.exe restore .\Unit4\
}

function Build {
    Restore
    msbuild.exe .\Unit4\Unit4.sln

    & ".\Unit4\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4\Unit4\bin\Debug\Unit4.exe --where="cat == RequiresExcelInstall"
}

function Test {
    & ".\Unit4\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4\Unit4\bin\Debug\Unit4.exe
}

function Run {
    & ".\Unit4\Unit4\bin\Debug\Unit4.exe"
}

export-modulemember -function Restore
export-modulemember -function Build
export-modulemember -function Test
export-modulemember -function Run