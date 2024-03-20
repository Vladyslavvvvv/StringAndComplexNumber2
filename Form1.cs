using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StringAndComplexNumber2
{
    // Custom interface inheriting from the base class
    public interface IString
    {
        byte Length();
        void Clear();
    }

    // IComparable and IClonable interfaces are implemented in the base class tring
    // Class representing the main form
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Method to check the validity of a complex number input
        private bool IsValidComplexNumber(string input)
        {
            // Divide the line by the "+" sign
            string[] parts = input.Split('+');

            // If after splitting, the string consists of two parts
            if (parts.Length == 2)
            {
                // Checking the first part for numbers
                if (parts[0].Any(char.IsDigit))
                {
                    // Checking the second part for numbers and the "i" symbol at the end
                    if (parts[1].TrimEnd().Any(char.IsDigit) && parts[1].TrimEnd().EndsWith("i"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Event handler for the button click
        private void buttonStart_Click(object sender, EventArgs e)
        {
            string firstInput = textBoxFirst.Text;
            string secondInput = textBoxSecond.Text;

            // Checking the correctness of entering the first number
            if (!IsValidComplexNumber(firstInput))
            {
                MessageBox.Show("Incorrect input format for first number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Checking the correctness of entering the second number
            if (!IsValidComplexNumber(secondInput))
            {
                MessageBox.Show("Incorrect input format for second number!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create complex numbers based on the entered lines
            ComplexNumber firstComplex = new ComplexNumber(firstInput);
            ComplexNumber secondComplex = new ComplexNumber(secondInput);

            // Check for equality
            bool isEqual = firstComplex.Equals(secondComplex);
            labelEquals.Text = "Equals? " + isEqual.ToString();

            // Adding numbers
            ComplexNumber sum = firstComplex.Add(secondComplex);
            labelAddition.Text = "Addition: " + sum.ToString();

            // Multiplication of numbers
            ComplexNumber product = firstComplex.Multiply(secondComplex);
            labelMultiplication.Text = "Multiplication: " + product.ToString();

            // Creating an array of objects
            ComplexNumber[] numbers = new ComplexNumber[4];

            // Filling the first half of the array
            numbers[0] = firstComplex;
            numbers[1] = secondComplex;

            // Filling the second half of the array with clones of the first half
            for (int i = 2; i < 4; i++)
            {
                numbers[i] = (ComplexNumber)numbers[i - 2].Clone();
            }

            try
            {
                // Sorting the array
                Array.Sort(numbers);

                // Displaying the sorted array using MessageBox
                string sortedNumbers = "Sorted Complex Numbers:\n";
                foreach (ComplexNumber num in numbers)
                {
                    sortedNumbers += num.ToString() + "\n";
                }
                MessageBox.Show(sortedNumbers, "Sorted Complex Numbers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Base class
    public abstract class tring : IComparable, ICloneable, IString
    {
        protected string value;
        protected byte length;

        public tring()
        {
            value = "";
            length = 0;
        }

        public tring(string str)
        {
            value = str;
            length = (byte)str.Length;
        }

        public tring(char ch)
        {
            value = ch.ToString();
            length = 1;
        }

        // Method to return the length of the string
        public byte Length()
        {
            return length;
        }

        // Abstract method to clear the string
        public abstract void Clear();

        // Method to compare objects
        public int CompareTo(object obj)
        {
            tring other = obj as tring;
            if (other == null)
            {
                MessageBox.Show("Object is not a tring");
            }
            return this.length.CompareTo(other.length);
        }

        // Method to clone an object
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        // Method to return the string representation of the object
        public override string ToString()
        {
            return value;
        }
    }

    // Class representing a complex number, inheriting from tring and implementing IComparable and ICloneable
    public class ComplexNumber : tring, IComparable, ICloneable
    {
        public ComplexNumber() : base()
        {
        }

        public ComplexNumber(string str) : base(str)
        {
        }

        // Method to clear the complex number
        public override void Clear()
        {
            value = "";
            length = 0;
        }

        // Method to check if two complex numbers are equal
        public bool Equals(ComplexNumber other)
        {
            return this.value.Equals(other.value);
        }

        // Method to add two complex numbers
        public ComplexNumber Add(ComplexNumber other)
        {
            double realPart1 = ExtractRealPart(this.value);
            double imagPart1 = ExtractImaginaryPart(this.value);
            double realPart2 = ExtractRealPart(other.value);
            double imagPart2 = ExtractImaginaryPart(other.value);

            double resultReal = realPart1 + realPart2;
            double resultImag = imagPart1 + imagPart2;

            return new ComplexNumber(resultReal.ToString() + "+" + resultImag.ToString() + "i");
        }

        // Method to multiply two complex numbers
        public ComplexNumber Multiply(ComplexNumber other)
        {
            double realPart1 = ExtractRealPart(this.value);
            double imagPart1 = ExtractImaginaryPart(this.value);
            double realPart2 = ExtractRealPart(other.value);
            double imagPart2 = ExtractImaginaryPart(other.value);

            double resultReal = (realPart1 * realPart2) - (imagPart1 * imagPart2);
            double resultImag = (realPart1 * imagPart2) + (imagPart1 * realPart2);

            return new ComplexNumber(resultReal.ToString() + "+" + resultImag.ToString() + "i");
        }

        // Method to extract the real part of a complex number
        private double ExtractRealPart(string complexNumber)
        {
            string[] parts = complexNumber.Split('+');
            return double.Parse(parts[0]);
        }

        // Method to extract the imaginary part of a complex number
        private double ExtractImaginaryPart(string complexNumber)
        {
            string[] parts = complexNumber.Split('+');
            string imagPart = parts[1].Substring(0, parts[1].Length - 1);
            return double.Parse(imagPart);
        }

        // Method to return the string representation of the object
        public override string ToString()
        {
            return value;
        }

        // Method to compare objects
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            ComplexNumber other = obj as ComplexNumber;
            if (other != null)
            {
                // Comparing complex numbers based on their values
                double thisRealPart = ExtractRealPart(this.value);
                double thisImagPart = ExtractImaginaryPart(this.value);
                double otherRealPart = ExtractRealPart(other.value);
                double otherImagPart = ExtractImaginaryPart(other.value);

                // Comparing real parts first
                int realComparison = thisRealPart.CompareTo(otherRealPart);
                if (realComparison != 0)
                    return realComparison;

                // If real parts are equal, comparing imaginary parts
                return thisImagPart.CompareTo(otherImagPart);
            }
            else
            {
                MessageBox.Show("Object is not a ComplexNumber");
                // If the type is not ComplexNumber, we cannot determine the sorting order,
                // so it's better to throw an exception rather than just displaying a message
                throw new ArgumentException("Object is not a ComplexNumber");
            }
        }

        // Method to clone an object
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}