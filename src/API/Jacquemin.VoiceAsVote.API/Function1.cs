using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using Jacquemin.VoiceAsVote.API.Options;
using Microsoft.Extensions.Options;

namespace Jacquemin.VoiceAsVote.API;

public class Function1
{
    public TableServiceClient TableServiceClient { get; }
    public VavTableStorageOptions Options { get; }

    public Function1(TableServiceClient tableServiceClient, IOptions<VavTableStorageOptions> options)
    {
        TableServiceClient = tableServiceClient;
        Options = options.Value;
    }

    [FunctionName("Function1")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        var tableClient = TableServiceClient.GetTableClient(Options.TableName);

        var rowsCount = await tableClient.QueryAsync<TableEntity>("PartitionKey eq 'MadeUpPartitionKey' and RowKey lt ''")
            .AsPages()
            .SelectMany(page => page.Values.ToAsyncEnumerable())
            .CountAsync();

        return new OkObjectResult($"Numbers of rows which matches a dummy impossible query: {rowsCount}");
    }
}