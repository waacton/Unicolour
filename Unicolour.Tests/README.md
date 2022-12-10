To generate test reports:

0. If `reportgenerator` is not installed, first run `'dotnet tool install -g dotnet-reportgenerator-globaltool'`
1. `dotnet test --test-adapter-path:. --logger:"nunit;LogFilePath=..\artifacts\{assembly}-test-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" --collect:"XPlat Code Coverage" --results-directory:".\artifacts"`
2. `reportgenerator "-reports:.\**\coverage.cobertura.xml" "-targetdir:.\artifacts\report" "-reporttypes:Html;TextSummary"`

