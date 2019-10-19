using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kalman.Data.SchemaObject;

namespace Kalman.Studio.Extensions
{
    internal class DataXGenerater
    {
        private static DataXGenerater m_Instance;

        public static DataXGenerater Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DataXGenerater();
                }
                return m_Instance;
            }
        }

        public SODatabase CurrDatabase { get; internal set; }

        private DataXGenerater() { }

        internal void GenTruncateScripts(Action<string> outputHandler)
        {
            if (DbSchemaHelper.Instance.CurrentSchema == null)
            {
                MsgBox.Show("请选择数据库");
                return;
            }

            var tables = this.CurrDatabase.TableList;
            StringBuilder sb = new StringBuilder();
            foreach (var item in tables)
            {
                sb.AppendLine($"truncate table {item.Name};");
            }
            outputHandler?.Invoke(sb.ToString());

            MsgBox.Show($"生成完毕，共{tables.Count}张表。");
        }

        internal void GenShellScripts(Action<string> outputHandler)
        {
            if (DbSchemaHelper.Instance.CurrentSchema == null)
            {
                MsgBox.Show("请选择数据库");
                return;
            }

            var tables = this.CurrDatabase.TableList;
            StringBuilder sb = new StringBuilder();
            foreach (var item in tables)
            {
                sb.AppendLine($"docker run --rm -it -v $PWD:/opt/datax/script ajayumi/datax /opt/datax/script/{item.Name}.json");
            }
            outputHandler?.Invoke(sb.ToString());

            MsgBox.Show($"生成完毕，共{tables.Count}张表。");
        }
    }
}
