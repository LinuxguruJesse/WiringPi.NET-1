﻿using System;
using System.Collections.Generic;
using WiringPiNet.Exceptions;
using WiringPiNet.Wrapper;

namespace WiringPiNet
{
	public class Gpio : IDisposable
	{
		public enum NumberingMode
		{
			Internal,
			Physical,
			System,
		}

		public NumberingMode PinNumberingMode { get; protected set; }

		public Gpio()
			: this(NumberingMode.Internal)
		{
		}

		public Gpio(NumberingMode mode)
		{
			int output = -1;
			switch (mode)
			{
				default:
				case NumberingMode.Internal:
					output = WiringPi.WiringPiSetup();
					break;
				case NumberingMode.Physical:
					output = WiringPi.WiringPiSetupPhys();
					break;
				case NumberingMode.System:
					output = WiringPi.WiringPiSetupSys();
					break;
			}

			PinNumberingMode = mode;

			if (output < 0)
			{
				throw new WiringPiNotAvailableException();
			}
		}

		public GpioPin GetPin(int pinNumber)
		{
			return new GpioPin(this, pinNumber);
		}

		public IEnumerable<GpioPin> GetPins(IEnumerable<int> pinNumbers)
		{
			foreach (int pinNumber in pinNumbers)
			{
				yield return GetPin(pinNumber);
			}
		}

		public IEnumerable<GpioPin> GetAllPins()
		{
			for (int i = 0; i <= 31; i++)
			{
				yield return new GpioPin(this, i);
			}
		}

		public void SetMode(int pin, PinMode mode)
		{
			WiringPi.PinMode(pin, (int)mode);
		}

		public PinMode GetMode(int pin)
		{
			return (PinMode)(WiringPi.GetAlt(pin));
		}

		public PinValue Read(int pin)
		{
			return (PinValue)WiringPi.DigitalRead(pin);
		}

		public void Write(int pin, PinValue value)
		{
			WiringPi.DigitalWrite(pin, (int)value);
		}

		public int ReadAnalog(int pin)
		{
			return WiringPi.AnalogRead(pin);
		}

		public void WriteAnalog(int pin, int value)
		{
			WiringPi.AnalogWrite(pin, value);
		}

		public void SetPullMode(int pin, PullMode mode)
		{
			WiringPi.PullUpDnControl(pin, (int)mode);
		}

		public void SetClock(int pin, int frequency)
		{
			WiringPi.GpioClockSet(pin, frequency);
		}

		public void WritePwm(int pin, int value)
		{
			WiringPi.PwmWrite(pin, value);
		}

		public void Dispose()
		{
		}
	}
}

