using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewSpotCode.Model;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using ViewSpotCode.ViewModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ViewSpotCode
{
    public class Function
    {
        public static string jPath = @"D:\CodingChallenge/mesh.json";
        /// <summary>
        /// Return N ViewSpots from Json Object
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<Element>> FunctionHandler(JObject input, ILambdaContext context)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var neighbors = await MeshModel.GetNeighboringElement(input);
            var viewSpots = MeshModel.GetViewSpots(neighbors, 10);

            watch.Stop();

            Console.WriteLine($"Loop 1 Execution Time: {watch.ElapsedMilliseconds} ms");

            return viewSpots;
            
           
        }

       

       
    }
}
