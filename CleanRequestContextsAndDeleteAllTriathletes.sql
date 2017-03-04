

delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='imchoo2016')

delete RequestContexts where raceId = 'imchoo2016'