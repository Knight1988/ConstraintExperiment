## Performance Test Report

Provider: Postgres

Test time: 2022/07/28 08:11:21

Customer: 1,000,000

Product: 100,000

Order: 1,000,000

Bellow is test performance report. Lower is better

|Test case|Constraint|Non Constraint|
|--|--|--|
|Search Product|1ms|1ms|
|Search Product|1ms|1ms|
|Search Product|1ms|1ms|
|Search Product|1ms|1ms|
|Search Product|1ms|1ms|
|Avg|1ms|1ms|
|Get revenue last month|600ms|542ms|
|Get revenue last month|670ms|487ms|
|Get revenue last month|610ms|508ms|
|Get revenue last month|679ms|505ms|
|Get revenue last month|636ms|623ms|
|Avg|639ms|533ms|
|Get revenue this year|1260ms|1230ms|
|Get revenue this year|1200ms|1334ms|
|Get revenue this year|1276ms|1271ms|
|Get revenue this year|1376ms|1279ms|
|Get revenue this year|1293ms|1226ms|
|Avg|1281ms|1268ms|
|Best seller product this year|1839ms|1975ms|
|Best seller product this year|1753ms|2028ms|
|Best seller product this year|1887ms|1908ms|
|Best seller product this year|1807ms|2097ms|
|Best seller product this year|1668ms|1820ms|
|Avg|1790.8ms|1965.6ms|
|Top customer this year|1884ms|1907ms|
|Top customer this year|1970ms|1976ms|
|Top customer this year|1986ms|1958ms|
|Top customer this year|1974ms|1856ms|
|Top customer this year|1953ms|1962ms|
|Avg|1953.4ms|1931.8ms|
|Insert customer|3ms|4ms|
|Insert customer|3ms|4ms|
|Insert customer|3ms|3ms|
|Insert customer|4ms|3ms|
|Insert customer|4ms|3ms|
|Avg|3.4ms|3.4ms|
|Update customer|4ms|5ms|
|Update customer|4ms|5ms|
|Update customer|4ms|4ms|
|Update customer|4ms|5ms|
|Update customer|4ms|4ms|
|Avg|4ms|4.6ms|
|Delete customer|4ms|5ms|
|Delete customer|4ms|4ms|
|Delete customer|4ms|4ms|
|Delete customer|6ms|4ms|
|Delete customer|5ms|4ms|
|Avg|4.6ms|4.2ms|
