using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

//namespace dnc01
namespace Asky
{
    public class HomeController : Controller
    {
        public async Task<string> index(int id, string name)
        {
            return await Task.FromResult($"askyms.com {id} {name}");
        }

        //插入一条数据 /home/DemoInsert?name=名称1  
        //返回1表示插入成功一条
        public async Task<string> DemoInsert(string name)
        {
            name = name + " askyms.com开源教程";
            return (await SqlA.Insert("insert into demo (name) values (@name)", new { name })).ToString();
        }

        //根据id查一条数据 /home/DemoSelect?id=2
        public async Task<string> DemoSelect(long id)
        {
            var demo = (await SqlA.Select<Demo>("select id, name from demo order by id where id=@id", new { id })).FirstOrDefault();

            if (demo != null)
                return demo.Id + " " + demo.Name;
            return "未找到数据";
        }

        //更新一条数据 /home/DemoUpdate?id=2&name=新名称2
        //返回1表示更新成功一条
        public async Task<string> DemoUpdate(long id, string name)
        {      
            return (await SqlA.Update("update demo set name=@name where id=@id", new { id, name })).ToString();
        }

        //删除一条数据 /home/DemoDelete?id=1
        //返回1表示删除成功一条
        public async Task<string> DemoDelete(long id)
        {
            return (await SqlA.Delete("delete from demo where id=@id", new { id })).ToString();
        }

    }
}