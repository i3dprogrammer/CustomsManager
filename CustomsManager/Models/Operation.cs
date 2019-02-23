using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsManager.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int Number { get; set; }
        public virtual ICollection<Section> Sections { get; set; }

        public void InitializeNewOperation()
        {
            if (Sections == null)
                Sections = new List<Section>();

            Sections.Add(new Section() { Name = "رسم الوارد", Receipt = null, Value = 0});
            Sections.Add(new Section() { Name = "Value Added Tax (VAT)  14%", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "ضريبة الارباح التجارية  0.5 %  من القيمة	",  Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف ادارية للجمارك", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف التفريغ", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "الخدمات التخزينية", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "الارضيات", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "غرامة تاخير", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف التحليل", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف النظافة", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف التنازل للتوكيل الملاحى", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "رسوم فحص الواردات", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "عمولة اصدار شيك مصرفى للجمارك", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "حراسة المطافى", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف ادارية للتوكيل الملاحى", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "رسوم هيئة الميناء والموازين", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = " نقل", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصروفات نثرية واقرار جمركي ودمغات", Receipt = null, Value = 0 });
            Sections.Add(new Section() { Name = "مصاريف واتعاب التخليص", Receipt = null, Value = 0 });
        }

        public float TotalCost()
        {
            float total = 0.0f;
            if(Sections != null)
            {
                Sections.ToList().ForEach(x =>
                {
                    total += x.Value;
                });
            }

            return total;
        }

        public float Total
        {
            get
            {
                return TotalCost();
            }
        }
    }
}
