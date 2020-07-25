using System;
using BeamErlRw.Beam;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SampleProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new BeamReaderOptions();
            options.OverrideLinesChunkToRaw(); //recommend not read LINES chunk, possible bugs

            var beamFile = BeamReader.Read("beamFiles/sockjs_action.beam", options);
            Console.WriteLine(beamFile);
            Console.WriteLine("======================");
            Console.WriteLine(beamFile.Header);
            Console.WriteLine("======================");
            Console.WriteLine(JsonConvert.SerializeObject(beamFile.Chunks, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            }));
            BeamWriter.Write("./sockjs_action.beam", beamFile);
        }
    }
}
