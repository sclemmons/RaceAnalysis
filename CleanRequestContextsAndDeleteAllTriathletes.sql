
select * from requestcontexts where raceid='IMBOULDER2017' order by AgeGroupId


delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='IMBOULDER2017')

delete RequestContexts where raceId = 'IMBOULDER2017' 

update Races
set ValidateMessage='mismatch' 
where raceid='IMBOULDER2017'