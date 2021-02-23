using System;
using System.Collections.Generic;
using RestSharp; //dotnet add package RestSharp

namespace TodoApiConsumer
{
    class Program
    {
        private const string BASEURL = "https://localhost:5001/api";

        static void Main(string[] args)
        {
            TodoItem item1 = GetItems(3);
            Console.WriteLine(item1.Name);

            List<TodoItem> list = GetItems();
            Console.WriteLine(list[0].Name);

            TodoItem item2 = new TodoItem { Name = "Guardar ropa", IsComplete = true };
            Console.WriteLine(item2);
            item2 = PostItem(item2);
            Console.WriteLine(item2);

            TodoItem item3 = GetItems(4);
            bool b = item3.IsComplete;
            item3.IsComplete = !b;
            PutItem(4, item3);
            item3 = GetItems(4);
            if (item3.IsComplete != b)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("NOK");
            }

            DeleteItem(5);
        }

        private static TodoItem GetItems(int id)
        {
            var client = new RestClient(BASEURL);
            var request = new RestRequest($"/TodoItems/{id}", Method.GET);
            var response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //Console.WriteLine(response.StatusCode);//NotFound|OK
            return TodoItem.FromJson(response.Content);
        }

        private static List<TodoItem> GetItems()
        {
            var client = new RestClient(BASEURL);
            var request = new RestRequest("TodoItems", Method.GET);
            var response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return TodoItem.ListFromJson(response.Content);
        }

        private static TodoItem PostItem(TodoItem item)
        {
            var client = new RestClient(BASEURL);
            var request = new RestRequest("TodoItems", Method.POST);
            //request.AddParameter("data", data);
            request.AddJsonBody(item.ToJson());
            var response = client.Execute(request);
            //Console.WriteLine(response.Content);
            //Console.WriteLine(response.StatusCode);//NotFound|Created
            return TodoItem.FromJson(response.Content);
        }

        private static void PutItem(int id, TodoItem item)
        {
            var client = new RestClient(BASEURL);
            // var request = new RestRequest("TodoItems", Method.PUT);
            // request.AddParameter("id", id);
            // request.AddParameter("data", data);
            var request = new RestRequest($"/TodoItems/{id}", Method.PUT);
            request.AddJsonBody(item.ToJson());
            var response = client.Execute(request);
            //Console.WriteLine(response.StatusCode);//NoContent|BadRequest
        }

        private static void DeleteItem(int id)
        {
            var client = new RestClient(BASEURL);
            var request = new RestRequest($"TodoItems/{id}", Method.DELETE);
            var response = client.Execute(request);
            //Console.WriteLine(response.StatusCode);//NotFound|NoContent
        }
    }
}
