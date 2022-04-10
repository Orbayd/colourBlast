
    public struct CellPosition
    {
        public int Row;
        public int Column;

        public CellPosition Left()
        {
            return new CellPosition(){ Row = this.Row -1 ,Column = Column};
        }

        public CellPosition Right()
        {
            return new CellPosition(){ Row = this.Row + 1 ,Column = Column};
        }

        public CellPosition Up()
        {
            return new CellPosition(){ Row = this.Row , Column = Column - 1 };
        }

        public CellPosition Down()
        {
            return new CellPosition(){ Row = this.Row, Column = Column + 1};
        }


    }

