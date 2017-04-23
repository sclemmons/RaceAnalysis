
select * from requestcontexts where raceid="imtx2013'


delete Triathletes 
where RequestContextId in
(select RequestContextId from RequestContexts 
where raceid='imtx2013')

delete RequestContexts where raceId = 'imtx2013'