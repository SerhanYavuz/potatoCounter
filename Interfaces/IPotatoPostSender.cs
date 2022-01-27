using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PotatoCounter.Models;

namespace PotatoCounter.Interfaces
{
    public interface IPotatoPostSender
    {
        public void SendPotatoPost(Potato potato);
    }
}