
select * from requestcontexts where raceid='himral2016' order by AgeGroupId


delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='himral2015')

delete RequestContexts where raceId = 'himral2015' 