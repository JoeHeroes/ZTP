﻿using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public interface IAnswerBuilder
    {
        public Word BuildWord();

        public void GetResult();
    }
}