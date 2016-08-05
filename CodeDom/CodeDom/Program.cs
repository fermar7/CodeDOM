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

//User can input code Example: Console.WriteLine("Hello");
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
                        
//Just a Test Class in Programm in a Programm

                        class Test {

                            public Test() {
                                Console.WriteLine(""Hello from Test!"");
                            }

                        }";


//Build Source string
            string source = SourcHeader + code + SourceFooter;

            Console.WriteLine(source);
            Console.ReadLine();

            Assembly assembly = CompileSource(source);
            
//Runs the Run-Method defined through the IRun interface 
//Look at source string

            IRun run = assembly?.CreateInstance("RunClass") as IRun;

            if (run != null) {
                run.Run();
            }
            Console.ReadLine();
        }


//Compiles the source string using hardcore stuff
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
