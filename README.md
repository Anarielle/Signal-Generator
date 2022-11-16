# Signal Generator
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/561dd59e468546138fe62c7d0be50caf)](https://www.codacy.com/gh/Anarielle/SignalGenerator/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Anarielle/SignalGenerator&amp;utm_campaign=Badge_Grade)

<div align="center">

![Alt text](./Resources/SG_realtime.gif)</div>

## Introduction
The signal generator allows you to generate a signal of various shapes and adjust the frequency, amplitude and phase as you want.
The main screen displays graphs of the original signal, harmonics, the received signal and the spectrum of the original signal (there are problems with the last two).
Using the switches and sliders you can adjust the waveform, the graphs will be updated immediately after making changes.

<div align="center">

![Alt text](./Resources/SG_harmonic.png)</div>

## Features
A few things you can do in the signal generator:
-   Select the shape of the generated signal (harmonic, triangular, rectangular)
-   Change the signal parameters using the sliders or the text field next to them
-   Scale, copy and save resulting graphics
-   View plotting in real time
-   Change graph parameters in real time

<div align="center">

![Alt text](./Resources/SG_functional.gif)</div>

## Technologies
-   .NET Fraemwork 4.8 
-   WPF
-   C#
-   [ScottPlot](https://scottplot.net/ "Go to the ScottPlot website") library version 4.1.45

## Project status
In the future, it is planned to refine the application: correct the display of the signal spectrum, change/add a spectrogram graph, adding the ability to display audio signal graphs from a microphone.