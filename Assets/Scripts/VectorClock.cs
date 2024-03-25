using System;
using System.Collections.Generic;
using UnityEngine;

namespace VectorClockNamespace
{
    public class VectorClock
    {
        private int myID;
        private Dictionary<int, int> clock;

        public VectorClock(int ID)
        {
            myID = ID;
            clock = new Dictionary<int, int>();
            clock.Add(myID, 0);
        }

        // Access ID
        public int MyID()
        {
            return myID;
        }

        // Access Clock
        public Dictionary<int, int> Clock()
        {
            return clock;
        }

        // Add a new process to the clock
        public void Add(int ID)
        {
            if (!clock.ContainsKey(ID))
            {
                clock.Add(ID, 0);
            }
        }

        // Remove a process from the clock
        public void Remove(int ID)
        {
            if (clock.ContainsKey(ID) && ID != myID)
            {
                clock.Remove(ID);
            } 
        }

        // Increment the local time for this process
        public void Tick()
        {
            clock[myID]++;
        }

        // Update the local time based on an external event
        public void ReceiveMessage(Dictionary<int, int> externalClock)
        {
            foreach (var kvp in externalClock)
            {
                Debug.Log("Contain key?" + kvp.Key + " " + clock.ContainsKey(kvp.Key));
                
                if (!clock.ContainsKey(kvp.Key) || kvp.Value > clock[kvp.Key])
                {
                    if (!clock.ContainsKey(kvp.Key))
                    {
                        clock.Add(kvp.Key, kvp.Value);
                    }
                    
                    clock[kvp.Key] = kvp.Value;
                    Debug.Log("After setting: " + clock[kvp.Key]);

                }
                else
                {
                    Debug.Log("Failed if: "+clock[kvp.Key]);
                }
                DisplayClock();
            }
        }

        // Compare this vector clock with another one
        public int CompareTo(VectorClock other)
        {
            bool greater = false;
            bool lesser = false;

            foreach (var kvp in clock)
            {
                if (!other.clock.ContainsKey(kvp.Key))
                {
                    return 0; 
                }
                else if (kvp.Value > other.clock[kvp.Key])
                {
                    greater = true;
                }
                else if (kvp.Value < other.clock[kvp.Key])
                {
                    lesser = true;
                }
            }

            if (greater && !lesser)
                return 1; 
            else if (!greater && lesser)
                return -1; 
            else
                return 0; 
        }

        // Display the vector clock
        public List<string> DisplayClock()
        {
            List<string> st = new List<string>();
            Debug.Log("Total count of processes: "+clock.Count);
            foreach (var kvp in clock)
            {
                Debug.Log($"Process {kvp.Key}: {kvp.Value}");
                st.Add($"Process {kvp.Key}: {kvp.Value}");
            }

            return st;
        }
    }
}
