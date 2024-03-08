# Optimizely Graph Client SDKs

Welcome to the Optimizely Graph Client SDK repository! This project is dedicated to building GraphQL queries with just a few lines of C# code, aimed at supporting Optimizely's customers in constructing search queries for [Optimizely Graph](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/project-graphql), similar to [Search & Navigation](https://docs.developers.optimizely.com/digital-experience-platform/v1.1.0-search-and-navigation/docs/net-client-api).
There are other alternatives that can be utilized to use the Optimizely Graph like [.Net GraphQL](https://github.com/graphql-dotnet/graphql-dotnet) and [Strawberry Shake](https://chillicream.com/docs/strawberryshake/)
## Getting Started

### Prerequisites

Before you contribute to this project, ensure you have the following installed:
- .NET SDK 6+
- SQL Server 2016 Express LocalDB ([download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads))
### Runing Tests locally
#### Run Unit tests
From repository level:
- Run `cd msbuild`
- Run `powershell .\unitTest.ps1` if you're using Command Line or `.\unitTest.ps1` if PowerShell

#### Run Integration tests
- Configure appsettings.json file in `.\APIs\src\Testing\EPiServer.ContentGraph.IntegrationTests` to connect to GraphQL gateway.
If appsettings.json was ready, from repository level:
- Run `cd msbuild`
- Run `powershell .\intergrationTest.ps1` if you're using Command Line or `.\intergrationTest.ps1` if PowerShell

### Run Alloy template site
- Configure appsettings.json file to connect to outside environments or your local environment. If you use your local environment, connect Alloy site to your local GraphQL gateway.
- Run `dotnet run --launch-profile "Kestrel (Env: Development)"`
- Go to [login](http://localhost:8000/Util/Login?ReturnUrl=/en/) page and login with account: `admin` and password: `Find@123` .
- Goto [Optimizely Graph content synchronization job](http://localhost:8000/EPiServer/EPiServer.Cms.UI.Admin/default#/ScheduledJobs/detailScheduledJob/2fafdd39-cd9c-4849-8338-fcb8d1824f3e) then click `Start`.
- After the job is completed, go to [Search page](http://localhost:8000/) and try your query.

### Run Foundation template site
- On the way...

### Optimizely Graph Client Tool
This tool supports for generating Optimizely Graph's schema to C# object models. You do not need to create models manually if install this tool.
#### Install Optimizely Graph Client Tool
From repository level:
- Run `mkdir ogtools`
- Run `cd ogtools`
- Run `dotnet new tool-manifest`
- Run `dotnet tool install Optimizely.Graph.Client.Tools --local`
- Run `dotnet ogschema path_to_your_appsettings.json path_to_store_your_models`
When the last command is succeed you will see the models will be generated in file `GraphModels.cs` located in the `path_to_store_your_models` you have ran in last command.

### Contributing

We welcome contributions from the community! Here's how you can contribute:

- **Fork the repository**: Start by forking the repository to your GitHub account.
- **Build and Test Locally**: Ensure you can build the project and run tests successfully on your local environment. It's crucial that you run all tests locally and achieve full test coverage before submitting a pull request, as external contributors won't have access to run tests using our GitHub actions without prior approval.
- **Pull Requests**: When you're ready, submit a pull request with your changes. Make sure your pull request description clearly describes the changes and the purpose, including if it fixes a bug or adds a new feature. Include the relevant issue number if applicable.
- **Running Tests**: Please ensure you have run all tests successfully locally and achieved full test coverage before submitting a pull request.

#### Did You Find a Bug?
- If the bug is a security vulnerability, please contact support@optimizely.com directly instead of opening up a GitHub issue.
- Check if the bug has already been reported by searching under [Issues](https://github.com/episerver/graph-net-sdk/issues) on GitHub.
- If you're unable to find an open issue addressing the problem, feel free to [open a new one](https://github.com/episerver/graph-net-sdk/issues/new). Please include a title and clear description, as much relevant information as possible, and a code sample or executable test case demonstrating the expected behavior that is not occurring.

#### Proposing a New Feature or Change
- We appreciate your suggestions for new features or changes to existing ones! To ensure alignment with the project's direction, please start by filing an issue and initiating a discussion before submitting a large pull request.

#### Questions About the Source Code
- If you have any questions about how to use the SDK or about the source code itself, join the conversation on the [Optimizely World forum](https://world.optimizely.com/forum/).

## Documentation
- [Optimizely Graph](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/project-graphql)
- [Optimizely Graph Client](https://docs.developers.optimizely.com/platform-optimizely/v1.4.0-optimizely-graph/docs/introduction)

Thank you for contributing to the Optimizely Graph Client SDK!
