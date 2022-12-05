using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Loan
    {
        // Variables that should be fixed (magic numbers)
        public double OriginationFeePercentage { get; set; } = 0.01;
        public double TaxesAndClosing { get; set; } = 2500.0;
        public double LoanInsuranceRatePercentage { get; set; } = 0.01;
        public double LoanRiskCoveragePercentage { get; set; } = 0.10;
        public int PaymentsPerYear { get; set; } = 12;
        public double PropertyTaxPercentage { get; set; } = 0.0125;
        public double HomeInsurancePercentage { get; set; } = 0.0075;

        // Variables to input for a loan calculation
        // All start as null. If a value is null then the
        // loan object is not ready to return loan information
        public int? LoanTerm { get; private set; } = null;
        public double? LoanInterestRate { get; set; } = null;
        public double? DownPayment { get; set; } = null;
        public double? PurchasePrice { get; set; } = null;   
        public double? HomeMarketValue { get; set; } = null;
        public double? HOAFeesYearly { get; set; } = null;

        // Variables used in calculation but not taken as input
        public double EquityPercentage { get; private set ; } = 0;
        public double EquityValue { get; private set; } = 0;
        public double LoanInsuranceAmountPerPayment { get; private set; } = 0;
        public double PropertyTaxPerPayment { get; private set; } = 0;
        public double HomeInsurancePerPayment { get; private set; } = 0;
        public double EscrowAmountPerPayment { get; private set; } = 0;
        public double TotalPaymentAmount { get; private set; } = 0;
        public double LoanBaseAmount { get; private set; } = 0;
        public double TotalLoanAmount { get; private set; } = 0;
        public double PrincpleMonthlyPayment { get; private set; } = 0;
        public double HOAFeesPerPayment { get; private set; } = 0;
        public double OriginationFeeValue { get; private set; } = 0;

        public bool IsLoanReady()
        {
            // check if there are any inputs that are still required
            if ((LoanTerm == null) || (PurchasePrice == null)
                 || (HomeMarketValue == null) || (DownPayment == null) || (HOAFeesYearly == null) 
                 || (LoanInterestRate == null))
            {
                return false;
            }
            return true;
        }
        public bool SetLoanTerm(int term)
        {
            if ((term == 15) || (term == 30))
            {
                LoanTerm = term;
                return true;
            }
            else { return false; }
        }
        public bool CalculatePrinciple()
        {
            // Requires Home purchase price and down payment to be set
            // home price minus down payment plus origination fee plus closing cost
            if (this.PurchasePrice == null || this.DownPayment == null)
            {
                return false;
            }
            else
            {
                double loanBaseAmount = (double)this.PurchasePrice - (double)this.DownPayment;
                this.LoanBaseAmount = loanBaseAmount;
                double originationFee = loanBaseAmount * this.OriginationFeePercentage;
                this.OriginationFeeValue = originationFee;
                double totalLoanAmount = (double)this.PurchasePrice - (double)this.DownPayment
                    + originationFee + this.TaxesAndClosing;
                this.TotalLoanAmount = totalLoanAmount;
                return true;
            }
            
        }
        public bool CalculatePrincipleMonthlyPayment()
        {
            // Requires TotalLoanAmount, LoanTerm, InterestRate
            if (this.LoanTerm == null || this.LoanInterestRate == null)
            {
                return false;
            }
            else
            {
                double p = (double)this.TotalLoanAmount;
                double r = (double)this.LoanInterestRate;
                double n = (double)this.PaymentsPerYear;
                double t = (double)this.LoanTerm;

                double monthlyPayment = (p
                    * (r / n)
                    * Math.Pow( (1+(r/n)) , (n*t) ))
                    / (Math.Pow( (1+(r/n)) , (n*t) ) - 1);

                this.PrincpleMonthlyPayment = monthlyPayment;
                return true;
            }


        }
        public bool CalculateEquity()
        {
            // Requires TotalLoanAmount, MarketPrice, PurchasePrice, DownPayment
            if (this.HomeMarketValue == null || this.PurchasePrice == null
                || this.DownPayment == null)
            {
                return false;
            }
            else
            {
                this.EquityValue = (double)this.HomeMarketValue - this.TotalLoanAmount;
                if (this.EquityValue <= 0)
                {
                    this.EquityPercentage = 0;
                }
                else {
                    this.EquityPercentage = this.EquityValue/ (double)this.HomeMarketValue;
                }
                return true;
            }
        }
        public bool IsLoanInsuranceRequired()
        {
            // Requires LoanRiskCoveragePercentage and HomeMarketValue
            if (this.EquityValue > (this.LoanRiskCoveragePercentage * this.HomeMarketValue))
            {
                this.LoanInsuranceAmountPerPayment = 0;
                return false;
            }
            else { return true; }
        }
        public bool CalculateMonthlyLoanInsurance()
        {
            // Requires TotalLoanAmount and LoanInsuranceRatePercentage
            // returns true for consistency
            this.LoanInsuranceAmountPerPayment = (this.TotalLoanAmount * this.LoanInsuranceRatePercentage)/this.PaymentsPerYear;
            return true;
        }
        public bool CalculateHOAFees()
        {
            if (this.HOAFeesYearly == null)
            {
                return false;
            }
            else
            {
                this.HOAFeesPerPayment = (double)this.HOAFeesYearly / this.PaymentsPerYear;
                return true;
            }
        }
        public bool CalculateEscrow()
        {
            // Requires home market value, property tax and homeowners insurance
            if (this.HomeMarketValue == null)
            {
                return false;
            }
            else
            {
                this.PropertyTaxPerPayment = ((double)this.HomeMarketValue * this.PropertyTaxPercentage) / this.PaymentsPerYear;
                this.HomeInsurancePerPayment = ((double)this.HomeMarketValue * this.HomeInsurancePercentage) / this.PaymentsPerYear;
                this.EscrowAmountPerPayment = this.PropertyTaxPerPayment + this.HomeInsurancePerPayment;
                return true;
            }
        }
        public bool CalculateTotalPayment()
        {
            //Requires principal monthly payment, loan insurance payment, hoa payments, escrow payments
            // returns true for consistency

            double totalPaymentAmt = (double)this.PrincpleMonthlyPayment + (double)this.LoanInsuranceAmountPerPayment
                + (double)this.HOAFeesPerPayment + (double)this.EscrowAmountPerPayment;

            this.TotalPaymentAmount = totalPaymentAmt;
            return true;
        }
        public bool CalculateLoanValues()
        {
            //TODO
            // First check if all inputs are there
            if (!IsLoanReady()) { return false; }
            // Perform calculations to update properties
            else
            {
                CalculatePrinciple();
                CalculatePrincipleMonthlyPayment();
                CalculateEquity();
                if (IsLoanInsuranceRequired())
                {
                    CalculateMonthlyLoanInsurance();
                }
                CalculateHOAFees();
                CalculateEscrow();
                CalculateTotalPayment();
                return true;
            }
        }
    }
}
