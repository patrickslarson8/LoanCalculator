namespace ConsoleApp1
{
    internal class Program
    {
        // Magic Numbers
        
        
        static void Main(string[] args)
        {
            // Internal logiv magic numbers
            double loanApprovalCutoffPercentage = 0.25;
            double income = -1;
            List<double> validLoanTerms = new List<double> { 15, 30 };

            // declare loan object with all nulls and fill in magic numbers
            Loan loan = new Loan();
            loan.PaymentsPerYear = 12;
            loan.OriginationFeePercentage  = 0.01;
            loan.TaxesAndClosing  = 2500.0;
            loan.LoanInsuranceRatePercentage  = 0.01;
            loan.LoanRiskCoveragePercentage  = 0.10;
            loan.PaymentsPerYear  = 12;
            loan.PropertyTaxPercentage  = 0.0125;
            loan.HomeInsurancePercentage  = 0.0075;

            UI.PrintMessage("Welcome to Pat Bank! We're happy to have your business.");

            // while loop, exit if no null values
            while (!loan.IsLoanReady())
            {
                // Get all inputs for loan
                GetLoanInputs(loan, validLoanTerms);

                UI.PrintMessage("Your loan is almost ready! We just need to make sure you can afford it.");
                if (income < 0)
                {
                    income = UI.GetUserInputAsDouble("Please enter your combined yearly income");
                }
                double monthlyIncome = GetIncomePerPayment(loan.PaymentsPerYear, income);

                // Prepare loan information
                loan.CalculateLoanValues();
                //UI.PrintMessage(PrepareLoanInformation(loan));
                PrintLoanInformation(loan);

                // Trick user into thinking these calculations are hard
                UI.PrintMessage("Determining if you are eligible...");
                System.Threading.Thread.Sleep(1000);

                // approve or deny loan (month income, month payment)
                bool approved = IsEligibleForLoan(loan, monthlyIncome, loanApprovalCutoffPercentage);
                if(approved)
                {
                    // approve:
                    UI.PrintMessage("You were approved! Congrats");
                    switch (UI.GetUserInputAsDouble(
                        "Choose an option: \n 1] Change your down payment\n 2] Change your home price\n Enter a number not on the menu to quit."))
                    {
                        // Changing a value to null will force loop to run again and prompt for that information
                        case 1:
                            loan.DownPayment = null;
                            break;
                        case 2:
                            loan.PurchasePrice = null;
                            break;
                        default: break;
                    }
                }
                else
                {
                    // deny: provide options
                    UI.PrintMessage("Your loan was denied.");
                    switch(UI.GetUserInputAsDouble(
                        "Choose an option: \n 1] Change your down payment\n 2] Change your home price\n Enter a number not on the menu to quit."))
                    {
                        // Changing a value to null will force loop to run again and prompt for that information
                        case 1:
                            loan.DownPayment = null;
                            break;
                        case 2:
                            loan.PurchasePrice = null;
                            break;
                        default: break;
                    }
                }
            }
        }
        public static void PrintLoanInformation(Loan loan)
        {
            UI.PrintDollars("Purchase Price: ",(double)loan.PurchasePrice);
            UI.PrintDollars("Current Market Value: ", (double)loan.HomeMarketValue);
            UI.PrintDollars("Down Payment: ", (double)loan.DownPayment);
            UI.PrintPercent("Interest Rate: ", (double)loan.LoanInterestRate);
            UI.PrintMessage($"Payments Per Year: {loan.PaymentsPerYear}");
            UI.PrintMessage($"Loan Term in Years: {loan.LoanTerm}");
            UI.PrintDollars("Yearly HOA Fees: ", (double)loan.HOAFeesYearly);
            UI.PrintDollars("Loan Base Amount: ", (double)loan.LoanBaseAmount);
            UI.PrintDollars("Origination Fee: ", (double)loan.OriginationFeeValue);
            UI.PrintDollars("Closing Costs: ", (double)loan.TaxesAndClosing);
            UI.PrintDollars("Total: ", (double)loan.TotalLoanAmount);
            UI.PrintPercent("Equity Percent: ",loan.EquityPercentage);
            UI.PrintDollars("Equity Value: ", (double)loan.EquityValue);
            UI.PrintDollars("Principal Monthly Payments: ", (double)loan.PrincpleMonthlyPayment);
            UI.PrintDollars("Loan Insurance Monthly: ", (double)loan.LoanInsuranceAmountPerPayment);
            UI.PrintDollars("HOA Fees Monthly: ", (double)loan.HOAFeesPerPayment);
            UI.PrintDollars("Taxes and Escrow Monthly: ", (double)loan.EscrowAmountPerPayment);
            UI.PrintDollars("Total Monthly Payments: ", (double)loan.TotalPaymentAmount);
        }
        public static bool IsEligibleForLoan(Loan loan, double monthlyIncome, double loanApprovalCutoffPercentage)
        {
            if (loan.TotalPaymentAmount >= (monthlyIncome*loanApprovalCutoffPercentage))
            {
                return false;
            }
            return true;
        }
        public static double GetIncomePerPayment(int paymentsPerYear, double yearlyIncome)
        {
            return yearlyIncome / paymentsPerYear;
        }
        public static void GetLoanInputs(Loan loan, List<double> validLoanTerms)
        {
            if (loan.LoanTerm == null) { loan.SetLoanTerm((int)UI.GetFencedUserInput(validLoanTerms,"Please enter the loan term in years [15/30]")); }
            if (loan.LoanInterestRate == null) { loan.LoanInterestRate = UI.GetInputAsPercentage("Plese enter the loan interest rate"); }
            if (loan.DownPayment == null) { loan.DownPayment = UI.GetUserInputAsDouble("Please enter the down payment in $USD with no commas"); }
            if (loan.PurchasePrice == null) { loan.PurchasePrice = UI.GetUserInputAsDouble("Please enter the purchase price of the home in $USD with no commas"); }
            if (loan.HomeMarketValue == null) { loan.HomeMarketValue = UI.GetUserInputAsDouble("Please enter the market value of the home in $USD with no commas"); }
            if (loan.HOAFeesYearly == null) { loan.HOAFeesYearly = UI.GetUserInputAsDouble("Please enter the yearly HOA fees of the home in $USD with no commas"); }
        }
    }
}