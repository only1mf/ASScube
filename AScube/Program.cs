using AScube;
using Swed32;
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Renderer renderer = new Renderer();
        Thread renderThread = new Thread(() => renderer.Start().Wait());
        renderThread.Start();

        // Init memory handler
        Swed swed = new Swed("ac_client");

        // Memory addresses
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
        int prevX = 0, prevY = 0, prevZ = 0;

        while (true)
        {
            swed.WriteFloat(baseModule, fovAdd, renderer.fovVal);
            Thread.Sleep(10);

            // Player Pos XYZ
            int currentX = swed.ReadInt(posX);
            int currentY = swed.ReadInt(posY);
            int currentZ = swed.ReadInt(posZ);

            if (currentX != prevX || currentY != prevY || currentZ != prevZ)
            {
                renderer.XVal = currentX;
                renderer.YVal = currentY;
                renderer.ZVal = currentZ;
                prevX = currentX;
                prevY = currentY;
                prevZ = currentZ;
                Console.Clear();
                Console.WriteLine($"{currentX},{currentY},{currentZ}");
            }

            if (renderer.checkBoxCords0) // Check if the checkbox is enabled
            {
                swed.WriteInt(posX, 1128809322);
                swed.WriteInt(posY, 1090519040);
                swed.WriteInt(posZ, 1101498080);
                // 1128809322,1090519040,1101498080
            }

            if (renderer.checkBoxInfAmmo) // Check if the checkbox is enabled
            {
                swed.WriteInt(ammoAdd, 69);
            }

            if (renderer.checkBoxInfClip) // Check if the checkbox is enabled
            {
                swed.WriteInt(clipAdd, 144);
            }

            Thread.Sleep(0); // Adjust the sleep time to control the update frequency
        }
    }
}
