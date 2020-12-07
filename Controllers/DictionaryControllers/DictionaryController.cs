using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pd_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.DictionaryControllers
{
    [Route("Dictionary")]
    public class DictionaryController : Controller
    {
        private AppDbContext context;

        public DictionaryController(AppDbContext ctx)
        {
            context = ctx;
        }

        [HttpGet("Dictionaries")]
        public IActionResult GetAllDictionaries()
        {
            IQueryable<pd_api.Models.DbModel.Dictionary> dictionaries = null;
            if (context.Dictionaries.Any())
            {
                dictionaries = context.Dictionaries;
            }
            return Ok(dictionaries);
        }

        [HttpGet]
        public async Task<IActionResult> GetDictionary(string dictionaryName)
        {
            pd_api.Models.DbModel.Dictionary dictionary = null;
            if (context.Dictionaries.Any())
            {
                dictionary = await context.Dictionaries.FirstOrDefaultAsync(d => d.Name == dictionaryName);
            }
            return Ok(dictionary);
        }

        [HttpPost]
        public IActionResult CreateDictionary(string name)
        {
            int currentUser = 123; //********************************* zmienić
            DateTime currentDate = DateTime.Now;

            Dictionary<string, string> dictionaryData = new Dictionary<string, string>();
            dictionaryData.Add("jeden", "1");

            pd_api.Models.DbModel.Dictionary dictionary = new pd_api.Models.DbModel.Dictionary
            {
                Name = name,
                DictionaryData = dictionaryData,
                UserCreateId = currentUser,
                CreateDate = currentDate
            };
            context.Dictionaries.Add(dictionary);
            context.SaveChangesAsync();
            return Created("/Dictionary", name);
        }
    }
}
