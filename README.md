C# Double Pendulum Simulator

This is an interactive C# Windows Forms application that simulates the chaotic motion of a double pendulum. It provides a visual, real-time representation of the physics and allows for full customization of all physical parameters.

The simulation is built from the ground up, using physics equations (based on the Lagrangian method) to calculate the angular acceleration, velocity, and position of the bobs in each frame.

Core Features

Real-Time Physics: Watch the pendulum evolve based on the calculated physics, running on a fast-updating timer.

Full Parameter Control:

Use sliders to adjust the lengths (L1, L2) and initial angles (θ1, θ2).

Use text boxes to set the values for gravity (g), mass of the first bob (m1), and mass of the second bob (m2).

Interactive Positioning: Click and drag either of the two pendulum bobs (the middle or the tail) to set a new starting position. The application automatically calculates the corresponding angles.

Multi-Pendulum Mode: Enable "Multiple Pendulums" to run several simulations at once, each with a tiny offset in its initial angle. This is a powerful way to visualize chaotic divergence—watch how quickly identical starting conditions diverge into completely different paths.

Visualization Tools:

Show Trail: Renders the path of the final bob.

Trail Fade: An optional effect that makes the trail gradually fade out.

Hide Pendulums: Hides the pendulum arms and bobs to focus only on the trail.

Full Color Customization: Use text boxes to set the color of the background, the trail, the pendulum bobs (root, middle, tail), and the connecting lines.

Simulation Controls:

Start/Stop: Pause or resume the simulation.

Reset: Resets all parameters to their default values.

Re-Run: Restarts the simulation from the current set of parameters.

Seeded Randomization:

Click Randomise with an empty text box for a completely random setup.

Type a word or seed (e.g., "chaos") into the "Random Seed" box and click "Randomise" to get a unique, reproducible set of starting parameters.

Save & Load Settings:

Use the "Options" menu to save your current set of parameters (lengths, masses, angles, multi-pendulum settings) to a JSON file.

Load any previously saved JSON file to instantly restore that configuration.

Input Validation: All text boxes feature real-time validation, showing a green tick for valid input and a red cross for invalid input (e.g., non-numeric values or invalid color names).

Tech Stack

Language: C#

Framework: .NET (Windows Forms)

Dependencies: Newtonsoft.Json (for saving and loading settings)

How to Build

Clone this repository.

Open the .sln file in Visual Studio.

Ensure you have the .NET Framework installed (e.g., 4.7.2 or later, as specified in the project).

Right-click the solution in the Solution Explorer and select "Restore NuGet Packages" (this will install Newtonsoft.Json).

Build and run the project.
