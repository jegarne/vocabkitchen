using Npgsql;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkCore.Extensions;
using VkCore.Requests.Word;
using VkInfrastructure.Data;
using VkInfrastructure.RequestHandlers.Word;
using Xunit;

namespace VkInfrastructure.Test.Requests.Word
{
    public class CreateWordAudioRequestHandlerShould
    {

        [Fact]
        public async Task CreateWordAudio()
        {
            NpgsqlConnection conn = new NpgsqlConnection("");
            conn.Open();

            // Define a query returning a single row result set
            NpgsqlCommand command = new NpgsqlCommand("SELECT \"Word\" FROM \"Words\"", conn);

            // Execute the query and obtain the value of the first column of the first row
            //var reader = command.ExecuteReader();

            //var sut = new CreateWordAudioRequestHandler();

            //List<Task> tasks = new List<Task>();

            //while (reader.Read())
            //{
            //    var word = reader[0]?.ToString();

            //    var request = new CreateWordAudioRequest(word.RemoveNonAlphaNumeric());
            //    tasks.Add(sut.Handle(request, new CancellationToken()));
            //}

            //await Task.WhenAll(tasks);
        }
    }
}
