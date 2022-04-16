
    public struct CellPosition
    {
        public int Row {get; private set;}
        public int Column {get; private set;}

        public CellPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }

        
        public CellPosition Left =>  new CellPosition(){ Row = this.Row -1 ,Column = Column};
    
        public CellPosition Right => new CellPosition(){ Row = this.Row + 1 ,Column = Column};
        
        public CellPosition Up =>  new CellPosition(){ Row = this.Row , Column = Column - 1 };
        
        public CellPosition Down =>  new CellPosition(){ Row = this.Row, Column = Column + 1};


    }

