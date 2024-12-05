public class DateConversionService
{
  private readonly Dictionary<int, NepaliYearData> _dateMap;

  public DateConversionService()
  {
    _dateMap = InitializeDateMap();
  }

    public NepaliDate getCurrentDate()
    {
        return ConvertEnglishDateToNepaliDate(DateTime.Now);
    }
  private Dictionary<int, NepaliYearData> InitializeDateMap()
  {
    return new Dictionary<int, NepaliYearData>
    {
      {1970, new NepaliYearData("1913-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1971, new NepaliYearData("1914-04-13", new int[] {31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30})},
      {1972, new NepaliYearData("1915-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {1973, new NepaliYearData("1916-04-13", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {1974, new NepaliYearData("1917-04-13", new int[] {31, 31, 32, 30, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1975, new NepaliYearData("1918-04-12", new int[] {31, 31, 32, 32, 30, 31, 30, 29, 30, 29, 30, 30})},
      {1976, new NepaliYearData("1919-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {1977, new NepaliYearData("1920-04-13", new int[] {30, 32, 31, 32, 31, 31, 29, 30, 29, 30, 29, 31})},
      {1978, new NepaliYearData("1921-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1979, new NepaliYearData("1922-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {1980, new NepaliYearData("1923-04-13", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {1981, new NepaliYearData("1924-04-13", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {1982, new NepaliYearData("1925-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1983, new NepaliYearData("1926-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {1984, new NepaliYearData("1927-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {1985, new NepaliYearData("1928-04-13", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {1986, new NepaliYearData("1929-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1987, new NepaliYearData("1930-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {1988, new NepaliYearData("1931-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {1989, new NepaliYearData("1932-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1990, new NepaliYearData("1933-04-13", new int[] {30, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1991, new NepaliYearData("1934-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {1992, new NepaliYearData("1935-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 30})},
      {1993, new NepaliYearData("1936-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1994, new NepaliYearData("1937-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1995, new NepaliYearData("1938-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1996, new NepaliYearData("1939-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1997, new NepaliYearData("1940-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1998, new NepaliYearData("1941-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {1999, new NepaliYearData("1942-04-13", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2000, new NepaliYearData("1943-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 29, 31})},
      {2001, new NepaliYearData("1944-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2002, new NepaliYearData("1945-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2003, new NepaliYearData("1946-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2004, new NepaliYearData("1947-04-14", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2005, new NepaliYearData("1948-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2006, new NepaliYearData("1949-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2007, new NepaliYearData("1950-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2008, new NepaliYearData("1951-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31})},
      {2009, new NepaliYearData("1952-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2010, new NepaliYearData("1953-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2011, new NepaliYearData("1954-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2012, new NepaliYearData("1955-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {2013, new NepaliYearData("1956-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2014, new NepaliYearData("1957-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2015, new NepaliYearData("1958-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2016, new NepaliYearData("1959-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {2017, new NepaliYearData("1960-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2018, new NepaliYearData("1961-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2019, new NepaliYearData("1962-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2020, new NepaliYearData("1963-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2021, new NepaliYearData("1964-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2022, new NepaliYearData("1965-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {2023, new NepaliYearData("1966-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2024, new NepaliYearData("1967-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2025, new NepaliYearData("1968-04-13", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2026, new NepaliYearData("1969-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2027, new NepaliYearData("1970-04-14", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2028, new NepaliYearData("1971-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2029, new NepaliYearData("1972-04-13", new int[] {31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30})},
      {2030, new NepaliYearData("1973-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2031, new NepaliYearData("1974-04-14", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2032, new NepaliYearData("1975-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2033, new NepaliYearData("1976-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2034, new NepaliYearData("1977-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2035, new NepaliYearData("1978-04-14", new int[] {30, 32, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31})},
      {2036, new NepaliYearData("1979-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2037, new NepaliYearData("1980-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2038, new NepaliYearData("1981-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2039, new NepaliYearData("1982-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {2040, new NepaliYearData("1983-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2041, new NepaliYearData("1984-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2042, new NepaliYearData("1985-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2043, new NepaliYearData("1986-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {2044, new NepaliYearData("1987-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2045, new NepaliYearData("1988-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2046, new NepaliYearData("1989-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2047, new NepaliYearData("1990-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2048, new NepaliYearData("1991-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2049, new NepaliYearData("1992-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {2050, new NepaliYearData("1993-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2051, new NepaliYearData("1994-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2052, new NepaliYearData("1995-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2053, new NepaliYearData("1996-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {2054, new NepaliYearData("1997-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2055, new NepaliYearData("1998-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2056, new NepaliYearData("1999-04-14", new int[] {31, 31, 32, 31, 32, 30, 30, 29, 30, 29, 30, 30})},
      {2057, new NepaliYearData("2000-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2058, new NepaliYearData("2001-04-14", new int[] {30, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2059, new NepaliYearData("2002-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2060, new NepaliYearData("2003-04-14", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2061, new NepaliYearData("2004-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2062, new NepaliYearData("2005-04-14", new int[] {30, 32, 31, 32, 31, 31, 29, 30, 29, 30, 29, 31})},
      {2063, new NepaliYearData("2005-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2064, new NepaliYearData("2007-04-14", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2065, new NepaliYearData("2008-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2066, new NepaliYearData("2009-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 29, 31})},
      {2067, new NepaliYearData("2010-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2068, new NepaliYearData("2011-04-14", new int[] {31, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2069, new NepaliYearData("2012-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2070, new NepaliYearData("2013-04-14", new int[] {31, 31, 31, 32, 31, 31, 29, 30, 30, 29, 30, 30})},
      {2071, new NepaliYearData("2014-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2072, new NepaliYearData("2015-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2073, new NepaliYearData("2016-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 31})},
      {2074, new NepaliYearData("2017-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2075, new NepaliYearData("2018-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2076, new NepaliYearData("2019-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {2077, new NepaliYearData("2020-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2078, new NepaliYearData("2021-04-14", new int[] {31, 31, 31, 32, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2079, new NepaliYearData("2022-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 29, 30, 30})},
      {2080, new NepaliYearData("2023-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 29, 30, 30})},
      {2081, new NepaliYearData("2024-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 29, 31})},
      {2082, new NepaliYearData("2025-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2083, new NepaliYearData("2026-04-14", new int[] {31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2084, new NepaliYearData("2027-04-14", new int[] {31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2085, new NepaliYearData("2028-04-13", new int[] {31, 32, 31, 32, 31, 31, 30, 30, 29, 30, 30, 30})},
      {2086, new NepaliYearData("2029-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2087, new NepaliYearData("2030-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 30, 29, 30, 30, 30})},
      {2088, new NepaliYearData("2031-04-15", new int[] {30, 31, 32, 32, 30, 31, 30, 30, 29, 30, 30, 30})},
      {2089, new NepaliYearData("2032-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2090, new NepaliYearData("2033-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2091, new NepaliYearData("2034-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 30, 29, 30, 30, 30})},
      {2092, new NepaliYearData("2035-04-13", new int[] {31, 31, 32, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2093, new NepaliYearData("2036-04-14", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2094, new NepaliYearData("2037-04-14", new int[] {31, 31, 32, 31, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2095, new NepaliYearData("2038-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 30, 30, 30, 30})},
      {2096, new NepaliYearData("2039-04-15", new int[] {30, 31, 32, 32, 31, 30, 30, 29, 30, 29, 30, 30})},
      {2097, new NepaliYearData("2040-04-13", new int[] {31, 32, 31, 32, 31, 30, 30, 30, 29, 30, 30, 30})},
      {2098, new NepaliYearData("2041-04-14", new int[] {31, 31, 32, 31, 31, 31, 29, 30, 29, 30, 30, 31})},
      {2099, new NepaliYearData("2042-04-14", new int[] {31, 31, 32, 31, 31, 31, 30, 29, 29, 30, 30, 30})},
      {2100, new NepaliYearData("2043-04-14", new int[] {31, 32, 31, 32, 30, 31, 30, 29, 30, 29, 30, 30})}
    };
  }

  public DateTime ConvertNepaliDateToEnglishDate(NepaliDate nepaliDate)
  {
    if (!_dateMap.ContainsKey(nepaliDate.Year))
      throw new ArgumentException($"Year {nepaliDate.Year} is not available in the date map.");

    var yearData = _dateMap[nepaliDate.Year];
    var firstDayOfBaisakh = DateTime.Parse(yearData.FirstBaisakh);
    var daysInMonth = yearData.DaysInMonth;

    if (nepaliDate.Month < 1 || nepaliDate.Month > 12)
      throw new ArgumentOutOfRangeException(nameof(nepaliDate.Month), "Month must be between 1 and 12.");
    if (nepaliDate.Day < 1 || nepaliDate.Day > daysInMonth[nepaliDate.Month - 1])
      throw new ArgumentOutOfRangeException(nameof(nepaliDate.Day), $"Day must be between 1 and {daysInMonth[nepaliDate.Month - 1]} for month {nepaliDate.Month}.");

    int totalDays = 0;
    for (int i = 0; i < nepaliDate.Month - 1; i++)
    {
      totalDays += daysInMonth[i];
    }
    totalDays += nepaliDate.Day - 1;

    return firstDayOfBaisakh.AddDays(totalDays);
  }

  public NepaliDate ConvertEnglishDateToNepaliDate(DateTime englishDate)
  {
    foreach (var kvp in _dateMap)
    {
      var year = kvp.Key;
      var yearData = kvp.Value;
      var firstDayOfBaisakh = DateTime.Parse(yearData.FirstBaisakh);

      if (englishDate < firstDayOfBaisakh) continue;

      int totalDays = (englishDate - firstDayOfBaisakh).Days;
      var daysInMonth = yearData.DaysInMonth;

      for (int month = 1; month <= 12; month++)
      {
        if (totalDays < daysInMonth[month - 1])
        {
          return new NepaliDate
          {
            Year = year,
            Month = month,
            Day = totalDays + 1
          };
        }
        totalDays -= daysInMonth[month - 1];
      }
    }

    throw new ArgumentException("The date is out of range for the date map.");
  }

  public int DaysInMonth(int year, NepaliMonth month)
  {
    if (!_dateMap.ContainsKey(year))
      throw new ArgumentException($"Year {year} is not available in the date map.");

    var daysInMonth = _dateMap[year].DaysInMonth;
    return daysInMonth[(int)month];
  }
  private class NepaliYearData
  {
    public string FirstBaisakh { get; }
    public int[] DaysInMonth { get; }

    public NepaliYearData(string firstBaisakh, int[] daysInMonth)
    {
      FirstBaisakh = firstBaisakh;
      DaysInMonth = daysInMonth;
    }
  }
}
