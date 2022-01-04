using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewSpotCode.Model;

namespace ViewSpotCode.ViewModel
{
    public class MeshModel
    {
       
        /// <summary>
        /// Get element Value from JObject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<Element> GetElementValues(JObject input)
        {
            JArray meshValues = input["values"].Value<JArray>();
            //var result = JsonConvert.DeserializeObject<List<Element>>(meshValues);
            List<Element> elementFirst = meshValues.ToObject<List<Element>>(); ;

            return elementFirst;

        }

        /// <summary>
        /// Get Nodes from JObject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<Node> GetNodes(JObject input)
        {
            JArray meshValues = input["nodes"].Value<JArray>();
            List<Node> nodes = meshValues.ToObject<List<Node>>(); ;

            return nodes;
        }

        /// <summary>
        /// Get ElementNode from JObject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<ElementNode> GetElementNode(JObject input)
        {
            JArray meshValues = input["elements"].Value<JArray>();
            List<ElementNode> elementNodes = meshValues.ToObject<List<ElementNode>>(); 

            return elementNodes;
        }

       /// <summary>
       /// Get Element Value from Json File
       /// </summary>
       /// <param name="Path"></param>
       /// <param name="N"></param>
       /// <returns></returns>
        public async Task<List<Element>> GetNViewSpots(string Path, int N)
        {
            using (StreamReader reader = new StreamReader(Path))
            {
                var mesh = await reader.ReadToEndAsync();
                JObject meshObject = JObject.Parse(mesh);
                JArray meshValues = meshObject["values"].Value<JArray>();
                //var result = JsonConvert.DeserializeObject<List<Element>>(meshValues);
                List<Element> elementFirst = meshValues.ToObject<List<Element>>();
                return elementFirst;
            }

        }

        /// <summary>
        /// Return list of Element Value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<List<Element>> GetNeighboringElement(JObject input)
        {           
            List<int> allNeighborsIds = null;
            List<Element> meshElement = new List<Element>();

            List<Node> nodes = GetNodes(input);
            List<ElementNode> elementNodes = GetElementNode(input);
            List<Element> elements = GetElementValues(input);

           await Task.Run(()=> nodes.ForEach(node =>
            {
                //foreach(var node in nodes)
                //{
                allNeighborsIds = new List<int>();
                elementNodes.ForEach(eNode => { if (eNode.nodes.Contains(node.ID)) { allNeighborsIds.Add(eNode.id); } });
                if (allNeighborsIds.Count >= 2)
                {
                    List<Element> elementTemp = new List<Element>();
                    foreach (var item in allNeighborsIds)
                    {
                        Element el = elements.First(n => n.element_id == item);
                        elementTemp.Add(el);

                    }
                    var nElement = elementTemp.GroupBy(n => n.value).ToHashSet();
                    foreach (var nItem in nElement)
                    {
                        foreach (var item in nItem)
                        {
                            var userIds = meshElement.ToHashSet();
                            bool contains = meshElement.Contains(item);
                            if (contains) continue;
                            meshElement.Add(item);
                        }
                    }
                    
                }
            }));
            
            return meshElement;
        }
     public static List<Element> GetViewSpots(List<Element> elements, int N)
     {       
         List<Element> ListOfViewSpots = elements.OrderByDescending(item => item.value).Take(N).ToList();
         return ListOfViewSpots;
     }
}
}
