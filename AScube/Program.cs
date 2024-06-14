using AScube;
using Swed32;
using System;
using System.Net.Sockets;
using System.Threading;

class Program
{
    static void Main()
    {
        Renderer renderer = new Renderer();
        Thread renderThread = new Thread(() => renderer.Start().Wait());
        renderThread.Start();

        // init mem handler
        Swed swed = new Swed("ac_client");

        // write Fov (ac_client.exe+18A7CC)
        IntPtr baseModule = swed.GetModuleBase("ac_client.exe");
        IntPtr ammoAdd = swed.ReadPointer(baseModule, 0x0183828, 0x8, 0x28, 0x64, 0x30, 0x30, 0x64) + 0xBEC;
        IntPtr clipAdd = swed.ReadPointer(baseModule, 0x017E0A8) + 0x140;
        IntPtr posX = swed.ReadPointer(baseModule, 0x017E0A8) + 0x2C;
        IntPtr posY = swed.ReadPointer(baseModule, 0x017E0A8) + 0x30;
        IntPtr posZ = swed.ReadPointer(baseModule, 0x017E0A8) + 0x28;
        int fovAdd = 0x18A7CC;

        // Thread for updating game values
        Thread updateThread = new Thread(() => UpdateGameValues(swed, baseModule, fovAdd, ammoAdd, clipAdd, posX, posY, posZ, renderer));
        updateThread.Start();
    }

    static void UpdateGameValues(Swed swed, IntPtr baseModule, int fovAdd, IntPtr ammoAdd, IntPtr clipAdd, IntPtr posX, IntPtr posY, IntPtr posZ, Renderer renderer)
    {
        int prevPosX = 0, prevPosY = 0, prevPosZ = 0;

        while (true)
        {
            swed.WriteFloat(baseModule, fovAdd, renderer.fovVal);
            Thread.Sleep(10);

            // Player Pos XYZ
            int currPosX = swed.ReadInt(posX);
            int currPosY = swed.ReadInt(posY);
            int currPosZ = swed.ReadInt(posZ);

            if (currPosX != prevPosX || currPosY != prevPosY || currPosZ != prevPosZ)
            {
                Console.Clear();
                Console.WriteLine($"X:{currPosX}");
                Console.WriteLine($"Y:{currPosY}");
                Console.WriteLine($"Z:{currPosZ}");

                // Update previous positions
                prevPosX = currPosX;
                prevPosY = currPosY;
                prevPosZ = currPosZ;
            }

            if (renderer.checkBoxInfAmmo) // Check if the checkbox is enabled
            {
                swed.WriteInt(ammoAdd, 850);
            }

            if (renderer.checkBoxInfClip) // Check if the checkbox is enabled
            {
                swed.WriteInt(clipAdd, 144);
            }
        }
    }
}
