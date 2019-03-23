﻿using PC_Ripper_Benchmark.exception;
using PC_Ripper_Benchmark.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;
using static PC_Ripper_Benchmark.function.RipperTypes;

namespace PC_Ripper_Benchmark.function {

    /// <summary>
    /// The <see cref="CPUFunctions"/> class.
    /// <para> </para>
    /// Represents all the functions for 
    /// testing the CPU component. Includes single
    /// and multithreaded testing using various
    /// common data structures.
    /// <para>Author: <see langword="Anthony Jaghab"/> (c),
    /// all rights reserved.</para>
    /// </summary>

    public class CPUFunctions {

        #region Instance members (fields)

        /// <summary>
        /// A <see cref="RipperSettings"/> instance
        /// used to get information about the test
        /// parameters.
        /// </summary>
        private readonly RipperSettings rs;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Default constructor for the <see cref="CPUFunctions"/>
        /// class.
        /// <para>Requires a <see cref="RipperSettings"/> instance to
        /// pull various test parameters e.g. iterations per test. </para>
        /// </summary>
        /// <param name="rs">A <see cref="RipperSettings"/> instance by reference.</param>

        public CPUFunctions(ref RipperSettings rs) {
            this.rs = rs;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Runs the benchmarking test on the CPU
        /// with a particular <see cref="ThreadType"/>.
        /// </summary>
        /// <param name="threadType">The type of threading 
        /// for the test.</param>
        /// <returns>A new <see cref="CPUResults"/> instance
        /// containing the result.</returns>
        /// <exception cref="RipperThreadException"></exception>

        public CPUResults RunCPUBenchmark(ThreadType threadType) {
            var results = new CPUResults(this.rs);

            switch (threadType) {

                case ThreadType.Single: {
                    // runs task on main thread.
                    RunTestsSingle(ref results);
                    break;
                }

                case ThreadType.SingleUI: {
                    // runs task, and waits for results.

                    // local function instead of action, very similar to
                    // Action a = delegate () { RunTestsSingle(ref results); }; 
                    void a() { RunTestsSingle(ref results); }

                    Task t = new Task(a);

                    t.Start();
                    t.Wait(); // the placement of this wait will have to be outside of here.
                    // likely in MainWindow.

                    break;
                }

                case ThreadType.Multithreaded: {
                    break;
                }

                default: {
                    throw new RipperThreadException("Unknown thread type to call. " +
                        "public CPUResults RunCPUBenchmark(ThreadType threadType) " +
                        "in function.CPUFunctions ");
                }
            }

            return results;
        }

        /// <summary>
        /// Runs each test <see cref="RipperSettings.IterationsPerCPUTest"/> times.
        /// <para>Should be (<see cref="CPUResults.UniqueTestCount"/> * 
        /// <see cref="RipperSettings.IterationsPerCPUTest"/>)
        /// timespans in <see cref="CPUResults.TestCollection"/>.</para>
        /// </summary>
        /// <param name="results">The <see cref="CPUResults"/> by reference 
        /// to add the <see cref="TimeSpan"/>(s).</param>

        private void RunTestsSingle(ref CPUResults results) {
            for (byte b = 0; b < this.rs.IterationsPerCPUTest; b++) {
                results.TestCollection.Add(RunSuccessorship());
            }

            for (byte b = 0; b < this.rs.IterationsPerCPUTest; b++) {
                results.TestCollection.Add(RunBoolean());
            }

            for (byte b = 0; b < this.rs.IterationsPerCPUTest; b++) {
                results.TestCollection.Add(RunQueue());
            }

            for (byte b = 0; b < this.rs.IterationsPerCPUTest; b++) {
                results.TestCollection.Add(RunLinkedList());
            }

            for (byte b = 0; b < this.rs.IterationsPerCPUTest; b++) {
                results.TestCollection.Add(RunTree());
            }
        }

        private async Task<CPUResults> RunTestsMultithreaded() {

        }

        /// <summary>
        /// Runs a test on successorship.
        /// <para>Counts from 0 to N.</para>
        /// Outputs a <see cref="TimeSpan"/> 
        /// representing how long it takes this operation.
        /// </summary>

        private TimeSpan RunSuccessorship() {
            var sw = Stopwatch.StartNew();

            for (ulong i = 0; i < this.rs.IterationsSuccessorship; i++) { }

            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Runs a test on boolean logic.
        /// <para>Internally, generates 2 random numbers type of <see cref="int"/> and 
        /// compares them. Whichever is greater, it adds to an output list.</para>
        /// Outputs a <see cref="TimeSpan"/> 
        /// representing how long it takes this operation.
        /// </summary>

        private TimeSpan RunBoolean() {
            Random rnd = new Random();

            var sw = Stopwatch.StartNew();

            // Checks whether the left random number is bigger
            // than the right. Not storing the results of these
            // rnd numbers, not necessary.

            for (ulong i = 0; i < this.rs.IterationsBoolean; i++) {
                bool b1 = (rnd.Next() > rnd.Next() ? true : false);
            }

            sw.Stop();
            return sw.Elapsed;
        }


        /// <summary>
        /// Runs a test using linked lists.
        /// <para>Creates two <see cref="LinkedList{T}"/> and
        /// adds, removes, and searches random numbers of type 
        /// <see cref="int"/>.</para>
        /// Outputs a <see cref="TimeSpan"/> 
        /// representing how long it takes this operation.
        /// </summary>

        private TimeSpan RunLinkedList() {
            const int LARGEST_NUM = 10000;
            Random rnd = new Random();
            LinkedList<int> lst1 = new LinkedList<int>();
            LinkedList<int> lst2 = new LinkedList<int>();

            var sw = Stopwatch.StartNew();
            int choice;

            for (ulong i = 0; i < this.rs.IterationsLinkedList; i++) {
                choice = rnd.Next(0, 3);

                switch (choice) {
                    // add
                    case 0: {
                        int rndNum = rnd.Next(LARGEST_NUM);
                        lst1.AddLast(rndNum);
                        lst2.AddLast(rndNum);

                        break;
                    }
                    // remove
                    case 1: {
                        int rndNum = rnd.Next(LARGEST_NUM);
                        bool b1 = lst1.Remove(rndNum);
                        bool b2 = lst2.Remove(rndNum);

                        break;
                    }
                    // search
                    case 2: {
                        int rndNum = rnd.Next(LARGEST_NUM);
                        bool b1 = lst1.Contains(rndNum);
                        bool b2 = lst2.Contains(rndNum);

                        break;
                    }
                }
            }
            // finished adding, removing and searching from these
            // linked lists.

            sw.Stop();
            return sw.Elapsed;

        }

        /// <summary>
        /// Runs a test using Queues.
        /// <para>Creates two <see cref="Queue{T}"/> and
        /// adds, removes, and searches random numbers of type 
        /// <see cref="int"/>.</para>
        /// Outputs a <see cref="TimeSpan"/> 
        /// representing how long it takes this operation
        /// </summary>

        private TimeSpan RunQueue() {
            Random rnd = new Random();
            Queue<int> q1 = new Queue<int>();
            Queue<int> q2 = new Queue<int>();

            List<bool> lstResult = new List<bool>();
            var sw = Stopwatch.StartNew();
            int choice;

            for (ulong i = 0; i < this.rs.IterationsQueue; i++) {
                choice = rnd.Next(0, 3);

                switch (choice) {
                    // add
                    case 0: {
                        int rndNum = rnd.Next();
                        q1.Enqueue(rndNum);
                        q2.Enqueue(rndNum);
                        break;
                    }
                    // remove
                    case 1: {
                        try { // if queues are empty, can throw exceptions.
                            int i1 = q1.Dequeue();
                            int i2 = q2.Dequeue();
                        } catch (InvalidOperationException) {
                            break;
                        }

                        break;
                    }
                    // contains
                    case 2: {
                        int rndNum = rnd.Next();
                        bool b1 = q1.Contains(rndNum);
                        bool b2 = q2.Contains(rndNum);

                        break;
                    }
                }
            }
            // finished adding, removing and searching from these
            // queues.

            sw.Stop();
            return sw.Elapsed;
        }

        /// <summary>
        /// Runs a test using Trees.
        /// <para>Creates two <see cref="SortedSet{T}"/> and
        /// adds, removes, and searches random numbers of type 
        /// <see cref="int"/>.</para>
        /// Outputs a <see cref="TimeSpan"/> 
        /// representing how long it takes this operation.
        /// </summary>

        private TimeSpan RunTree() {
            Random rnd = new Random();
            SortedSet<string> set1 = new SortedSet<string>();
            SortedSet<string> set2 = new SortedSet<string>();

            var sw = Stopwatch.StartNew();
            int choice;

            for (ulong i = 0; i < this.rs.IterationsTree; i++) {
                choice = rnd.Next(0, 3);

                switch (choice) {
                    // add
                    case 0: {
                        string rndStr = rnd.Next().ToString();
                        set1.Add(rnd.Next().ToString());
                        set2.Add(rnd.Next().ToString());

                        break;
                    }
                    // remove
                    case 1: {
                        string rndStr = rnd.Next().ToString();
                        bool b1 = set1.Remove(rndStr);
                        bool b2 = set2.Remove(rndStr);

                        break;
                    }
                    // search
                    case 2: {
                        string rndStr = rnd.Next().ToString();
                        bool b1 = set1.Contains(rndStr);
                        bool b2 = set2.Contains(rndStr);

                        break;
                    }
                }
            }
            // finished adding, removing and searching from these
            // linked lists.

            sw.Stop();
            return sw.Elapsed;
        }
    }
    #endregion
}
