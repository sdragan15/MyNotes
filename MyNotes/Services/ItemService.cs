using MyNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotes.Services
{
    public class ItemService
    {
        List<Item> items = [];

        public ItemService()
        {

        }

        public List<Item> GetItems()
        {
            return items;
        }
    }
}
