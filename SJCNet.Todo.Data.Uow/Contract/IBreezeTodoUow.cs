using Breeze.WebApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJCNet.Todo.Data.Uow.Contract
{
    public interface IBreezeTodoUow : ITodoUow
    {
        SaveResult Commit(JObject changeSet);
    }
}
