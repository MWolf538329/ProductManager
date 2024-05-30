using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface ICategoryDAL
    {
        List<Category> GetCategories();
    }
}
