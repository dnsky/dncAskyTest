using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dnc01
{
    public class HomeController : Controller
    {   
        public async Task<string> index(int id, string name)
        {
            return await Task.FromResult($"askyms.com {id} {name}");
        }
    }
}