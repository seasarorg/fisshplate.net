using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.IO;
using Seasar.Fisshplate.Wrapper;
using NPOI.HSSF.UserModel;
using Seasar.Fisshplate.Parser;
using Seasar.Fisshplate.Core.Element;
using Seasar.Fisshplate.Parser.Handler;

namespace Seasar.Fisshplate.Test.Parser
{
    [TestFixture]
    public class FPParserTest
    {
        [Test]
        public void Test解析テスト1()
        {
            using (Stream s = new FileStream("FPTemplateTest.xls", FileMode.Open, FileAccess.Read))
            {
                WorkbookWrapper workbook = new WorkbookWrapper(new HSSFWorkbook(s));
                FPParser parser = new FPParser();

                Root root = parser.Parse(workbook.GetSheetAt(0));
                Assert.AreEqual(typeof(NullElement), root.PageHeader.GetType());
                Assert.AreEqual(typeof(NullElement), root.PageFooter.GetType());

                IList<TemplateElement> bodyList = root.BodyElementList;
                
                // 1行目
                TemplateElement row = bodyList[0];
                Assert.AreEqual(typeof(Row), row.GetType());
                IList<TemplateElement> cellList = ((Row)row).CellElementList;
                TemplateElement cell = cellList[0];
                Assert.AreEqual(typeof(GenericCell), cell.GetType());
                cell = cellList[1];
                Assert.AreEqual(typeof(NullCell), cell.GetType());

                // 2行目
                row = bodyList[1];
                cellList = ((Row)row).CellElementList;
                cell = cellList[0];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = cellList[2];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = cellList[3];
                Assert.AreEqual(typeof(GenericCell), cell.GetType());

                // 3行目
                row = bodyList[2];
                cellList = ((Row)row).CellElementList;
                cell = cellList[0];
                Assert.AreEqual(typeof(GenericCell), cell.GetType());
                cell = cellList[1];
                Assert.AreEqual(typeof(GenericCell), cell.GetType());
                cell = cellList[2];
                Assert.AreEqual(typeof(GenericCell), cell.GetType());

                // 4行目
                row = bodyList[3];
                Assert.AreEqual(typeof(IteratorBlock), row.GetType());
                IList<TemplateElement> childList = ((IteratorBlock)row).ChildList;
                Assert.AreEqual(1, childList.Count);
                TemplateElement child = (TemplateElement)childList[0];
                Assert.AreEqual(typeof(IfBlock), child.GetType());
    
                row = (TemplateElement)((IfBlock)child).ChildList[0];
                cellList = ((Row)row).CellElementList;
                cell = cellList[0];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = cellList[1];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = cellList[2];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = cellList[3];
                Assert.AreEqual(typeof(NullCell), cell.GetType());
                cell = cellList[4];
                Assert.AreEqual(typeof(NullCell), cell.GetType());
                cell = cellList[5];
                Assert.AreEqual(typeof(El), cell.GetType());

                TemplateElement next = ((IfBlock)child).NextBlock;
                Assert.AreEqual(typeof(ElseBlock), next.GetType());
                row = (TemplateElement)((ElseBlock)next).ChildList[0];
                cellList = ((Row)row).CellElementList;
                cell = (TemplateElement)cellList[0];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = (TemplateElement)cellList[1];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = (TemplateElement)cellList[2];
                Assert.AreEqual(typeof(El), cell.GetType());
                cell = (TemplateElement)cellList[3];
                Assert.AreEqual(typeof(El), cell.GetType());

                row = (TemplateElement)((ElseBlock)next).ChildList[1];
                Assert.AreEqual(typeof(PageBreak), row.GetType());

                // 5つ目（Not5行目）
                row = bodyList[4];
                Assert.AreEqual(typeof(PageBreak), row.GetType());

                // 6つ目（Not6行目）
                row = bodyList[5];
                Assert.AreEqual(typeof(IteratorBlock), row.GetType());
                childList = ((IteratorBlock)row).ChildList;
                Assert.AreEqual(2, childList.Count);
                child = (TemplateElement)childList[0];
                Assert.AreEqual(typeof(IfBlock), child.GetType());

                row = (TemplateElement)((IfBlock)child).ChildList[0];
                cellList = ((Row)row).CellElementList;
                Assert.AreEqual(typeof(GenericCell), cellList[0].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[1].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[2].GetType());
                Assert.AreEqual(typeof(El), cellList[3].GetType());

                next = ((IfBlock)child).NextBlock;
                Assert.AreEqual(typeof(ElseIfBlock), next.GetType());
                row = (TemplateElement)((ElseIfBlock)next).ChildList[0];
                cellList = ((Row)row).CellElementList;
                Assert.AreEqual(typeof(GenericCell), cellList[0].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[1].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[2].GetType());
                Assert.AreEqual(typeof(El), cellList[3].GetType());

                TemplateElement next2 = ((ElseIfBlock)next).NextBlock;
                Assert.AreEqual(typeof(ElseIfBlock), next.GetType());
                row = (TemplateElement)((ElseIfBlock)next2).ChildList[0];
                cellList = ((Row)row).CellElementList;
                Assert.AreEqual(typeof(GenericCell), cellList[0].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[1].GetType());
                Assert.AreEqual(typeof(GenericCell), cellList[2].GetType());
                Assert.AreEqual(typeof(El), cellList[03].GetType());

                child = (TemplateElement)childList[1];
                Assert.AreEqual(typeof(Row), child.GetType());
                cellList = ((Row)child).CellElementList;
                Assert.AreEqual(typeof(El), cellList[0].GetType());
                Assert.AreEqual(typeof(El), cellList[1].GetType());
                Assert.AreEqual(typeof(El), cellList[2].GetType());
                Assert.AreEqual(typeof(El), cellList[3].GetType());

                row = (TemplateElement)bodyList[6];
                Assert.AreEqual(typeof(VarElement), row.GetType());

                row = (TemplateElement)bodyList[7];
                Assert.AreEqual(typeof(Exec), row.GetType());

            }
        }

        [Test]
        public void Test_Suspend解析()
        {

            using (Stream s = new FileStream("FPTemplate_SuspendTest.xls", FileMode.Open, FileAccess.Read))
            {
                FPParser parser = new FPParser();

                WorkbookWrapper workbook = new WorkbookWrapper(new HSSFWorkbook(s));
                CellWrapper cellWrapper = workbook.GetSheetAt(0).GetRow(3).GetCell(3);

                CellParserHandler handler = new CellParserHandler();
                TemplateElement elem = handler.GetElement(cellWrapper);
                Assert.AreEqual(typeof(Suspend), elem.GetType());
                // Suspendとしてかいせきされている！

            }

        }
    }
}
