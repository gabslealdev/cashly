
export interface GetCashflowBoardResponse{
    cashflowId: string;
    title: string;
    userRole: string;
    months: CashflowBoardMonthResponse[];
}

export interface CashflowBoardMonthResponse{
    year: number;
    month: number;
    period: string;
    balance: number;
    projected: number;
    isClosed: boolean;
    financialHealthStatus: string;
    transactions: CashflowBoardTransactionResponse[];
}

export interface CashflowBoardTransactionResponse{
    transactionId: string;
    title: string;
    amount: number;
    type: string;
    date: string;
    status: string;
}