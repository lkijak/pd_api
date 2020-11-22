using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pd_api.Service;
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
            var dictionaries = context.Dictionaries;
            if (dictionaries.Any())     // ***************************** Sprawdzić warunek przy zapisanych słownikach w bazie
            {
                return Json(dictionaries);
            }
            //return Json(new { succeeded = false, messageInfo = MessageInfo.Dictionary_CouldNotFindAnyDictionary });
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetDictionary(string dictionaryName)
        {
            var dictionary = await context.Dictionaries.FirstOrDefaultAsync(d => d.Name == dictionaryName);
            if (dictionary != null)
            {
                return Json(dictionary);
            }
            //return Json(new { succeeded = false, messageInfo = MessageInfo.Dictionary_CouldNotFindDictionary });
            return NotFound(("Dupa {0}", dictionaryName));
        }


    }
}
