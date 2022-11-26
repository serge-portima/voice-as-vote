# voice-as-vote
Voting system which support expression of full expectation

# Setup

## Visual Studio

 - Authenticate Visual Studio with Azure ([Configure Visual Studio for Azure development with .NET](https://learn.microsoft.com/sr-cyrl-rs/dotnet/azure/configure-visual-studio#authenticate-visual-studio-with-azure))

 ## Azure resources

 - Create a Storage account then, within table storage, create a table ([Quickstart: Create an Azure Storage table in the Azure portal](https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-quickstart-portal))

 ## local.settings.json
When all the setup is completed, fill the following settings in with the correct values.
 - **TableStorageOptions__Endpoint**
 Endpoint of your table storage (typically something like: *https://&lt;some name&gt;.table.core.windows.net*)
 - **TableStorageOptions__TableName** Name of your table in table storage
