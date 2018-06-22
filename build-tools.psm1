function Restore {
    nuget.exe restore .
}

function Build {
    Restore
    msbuild.exe .\Unit4.sln

    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4.Automation.Tests\bin\Debug\Unit4.Automation.Tests.dll --where="cat == RequiresExcelInstall"
}

function Test {
    & ".\packages\NUnit.ConsoleRunner.3.8.0\tools\nunit3-console.exe" .\Unit4.Automation.Tests\bin\Debug\Unit4.Automation.Tests.dll
}

function Help {
    & ".\Unit4\bin\Debug\unit4-automation.exe" help
}

function Installer {
    & ".\packages\WiX.3.11.1\tools\candle.exe" .\unit4-automation.wxs
    & ".\packages\WiX.3.11.1\tools\light.exe" .\unit4-automation.wixobj -sice:ICE91
}

export-modulemember -function Restore
export-modulemember -function Build
export-modulemember -function Test
export-modulemember -function Run
export-modulemember -function Help
export-modulemember -function Installer