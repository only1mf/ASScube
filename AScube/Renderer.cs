using System;
using System.Diagnostics;
using System.Numerics;
using ClickableTransparentOverlay;
using ImGuiNET;

namespace AScube
{
    public class Renderer : Overlay
    {
        // Sliders & inputs values
        public float fovVal = 90; // FOV value
        private float fallFovVal = 90; // Fallback value

        // Check boxes 
        public bool checkBoxInfAmmo = false;
        public bool checkBoxInfClip = false;
        public bool checkBoxBhop = false;
        public bool checkBoxCords0 = false;

        // Position values
        public int XVal = 0;
        public int YVal = 0;
        public int ZVal = 0;

        // RGBA
        Vector4 colorVal = new Vector4(1, 1, 1, 1);
        Vector4 colorLinkGithub = new Vector4(0, 100, 255, 255);

        private bool stylesSet = false; // Flag to ensure styles are set only once

        protected override void Render()
        {
            if (!stylesSet)
            {
                SetImGuiStyle();
                stylesSet = true;
            }

            // Begin window
            ImGui.Begin("ASScube");

            // Get the current window position and size
            Vector2 windowPos = ImGui.GetWindowPos();
            Vector2 windowSize = ImGui.GetWindowSize();

            if (ImGui.BeginTabBar("Tab1"))
            {
                if (ImGui.BeginTabItem("Main"))
                {
                    ImGui.SliderFloat("Fov Changer", ref fovVal, 50, 160);
                    if (ImGui.Button("Reset fov"))
                    {
                        fovVal = fallFovVal;
                    }
                    ImGui.Checkbox("inf ammo", ref checkBoxInfAmmo);
                    ImGui.Checkbox("inf clip", ref checkBoxInfClip);
                    ImGui.Checkbox("bhop", ref checkBoxBhop);
                    ImGui.Checkbox("Set Coords to 1", ref checkBoxCords0);

                    ImGui.Separator();

                    // Set a fixed size for the input boxes
                    ImGui.PushItemWidth(100);
                    ImGui.InputInt("X", ref XVal);
                    ImGui.SameLine();
                    ImGui.InputInt("Y", ref YVal);
                    ImGui.SameLine();
                    ImGui.InputInt("Z", ref ZVal);
                    ImGui.PopItemWidth(); // Reset the item width to its default value

                    ImGui.Separator();

                    ImGui.ColorEdit4("color", ref colorVal);
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Info"))
                {
                    ImGui.Text("Assault cube cheeto v1304");
                    ImGui.TextColored(colorLinkGithub, "https://github.com/only1mf");
                    if (ImGui.IsItemHovered() && ImGui.IsItemClicked())
                    {
                        OpenUrl("https://github.com/only1mf");
                    }
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }

            // End window
            ImGui.End();

            // Draw outline around the whole window
            DrawOutline(windowPos, windowSize);
        }

        private void SetImGuiStyle()
        {
            var style = ImGui.GetStyle();
            var colors = style.Colors;

            // Set window background color
            colors[(int)ImGuiCol.WindowBg] = new Vector4(0.1f, 0.1f, 0.1f, 0.8f);

            // Set other colors
            colors[(int)ImGuiCol.TitleBg] = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.TitleBgActive] = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.Tab] = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.TabActive] = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.Button] = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.ButtonHovered] = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.ButtonActive] = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
            colors[(int)ImGuiCol.FrameBg] = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            colors[(int)ImGuiCol.FrameBgHovered] = new Vector4(0.3f, 0.3f, 0.3f, 1.0f);
            colors[(int)ImGuiCol.FrameBgActive] = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);

            // Set rounding for buttons and other elements
            style.FrameRounding = 5.0f;
            style.GrabRounding = 5.0f;
            style.WindowRounding = 5.0f;

            // Set padding and spacing
            style.WindowPadding = new Vector2(10, 10);
            style.FramePadding = new Vector2(5, 5);
            style.ItemSpacing = new Vector2(10, 10);
        }

        private void DrawOutline(Vector2 windowPos, Vector2 windowSize)
        {
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddRect(windowPos, windowPos + windowSize, ImGui.ColorConvertFloat4ToU32(new Vector4(0, 255, 0, 1)), 5.0f, ImDrawFlags.None, 2.0f);
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open URL: {ex.Message}");
            }
        }
    }
}
