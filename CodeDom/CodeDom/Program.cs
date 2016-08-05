using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom.Compiler;
using System.CodeDom;
using Microsoft.CSharp;
using System.IO;
using System.Reflection;
using DomLib;

namespace CodeDom {

    class Program {

        static void Main(string[] args) {

            while (true) {
                Console.WriteLine("Code:");
                Test1(Console.ReadLine());
            }


        }


        private static void Test1(string code) {

            string SourcHeader = @"
                        using System;
                        class RunClass : DomLib.IRun {
                            public void Run() {";

            string SourceFooter = @"
                            }
                        }

                        class Test {

                            public Test() {
                                Console.WriteLine(""Hello from Test!"");
                            }

                        }";

            string source = SourcHeader + code + SourceFooter;

            Console.WriteLine(source);
            Console.ReadLine();

            Assembly assembly = CompileSource(source);

            IRun run = assembly?.CreateInstance("RunClass") as IRun;

            if (run != null) {
                run.Run();
            }
            Console.ReadLine();
        }


        private static Assembly CompileSource(string sourceCode) {

            CodeDomProvider cpd = new CSharpCodeProvider();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("DomLib.dll");
            cp.GenerateExecutable = false;
            CompilerResults cr = cpd.CompileAssemblyFromSource(cp, sourceCode);

            if (cr.Errors.Count != 0) {
                foreach (var item in cr.Errors) {
                    Console.WriteLine(item);
                }
                Console.ReadLine();
                return null;
            }

            return cr.CompiledAssembly;
        }
    }
}
