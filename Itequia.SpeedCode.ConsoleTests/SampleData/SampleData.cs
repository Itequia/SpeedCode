using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itequia.Backend.ConsoleTests.SampleData
{
    public class SampleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Created { get; set; }

        public SampleEntity(int _id, string _name, int _age)
        {
            this.Id = _id;
            this.Name = _name;
            this.Age = _age;
            this.Created = DateTime.Now;
        }
    }
    public static class SampleData
    {
        public static IEnumerable<SampleEntity> GenerateSampleList(int itemsCount=100)
        {
            List<SampleEntity> sampleEntities = new List<SampleEntity>();
            for(int i = 0; i < itemsCount; i++)
            {
                sampleEntities.Add(new SampleEntity(i + 1, "AutoName" + i, (i + 1) * 2));
            }

            return sampleEntities;
        }

    }
}
