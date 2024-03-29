Tests to add / update for new colour spaces:
- Smoke tests
- Known value conversion tests (if data available, e.g. Oklab, Hsluv)
- Roundtrip conversion tests
- Mixing tests
- Greyscale mixing tests (if colour space has hue component)
- Hued tests (if colour space has hue component)
- Equality tests
- Extreme values tests
- Greyscale tests
- NaN tests
- Difference tests (if colour space has corresponding ΔE)
- Configuration tests (if colour space requires configuration)
- Lazy evaluation tests

To obtain code coverage metrics (as GitLab CI is setup to do):

0. Install `reportgenerator` if not already installed <br/>`
dotnet tool install -g dotnet-reportgenerator-globaltool'`
1. Run tests and output artifacts <br/>
`dotnet test --test-adapter-path:. --logger:"junit;LogFilePath=..\artifacts\{assembly}-test-result.xml;MethodFormat=Class;FailureBodyFormat=Verbose" --collect:"XPlat Code Coverage" --results-directory:".\artifacts"`
2. Generate report from artifacts <br/>
`reportgenerator "-reports:.\**\coverage.cobertura.xml" "-targetdir:.\artifacts\report" "-reporttypes:Html;TextSummary"`